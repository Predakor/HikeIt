import { RMarker, RPopup } from "maplibre-react-components";
import { useState, type ReactNode } from "react";
import type { HoverTarget } from "../MapData/mapData.types";
import { useIsHovered, useSetHovered } from "../MapData/hooks";

interface HoverMarkerProps {
  kind: HoverTarget["kind"];
  id: number;
  longitude: number;
  latitude: number;
  children: (isHovered: boolean) => ReactNode;
  popup?: ReactNode;
}

// generic marker: hover is shared through the hover store, click toggles the optional popup
export default function HoverMarker({
  kind,
  id,
  longitude,
  latitude,
  children,
  popup,
}: HoverMarkerProps) {
  const isHovered = useIsHovered(kind, id);
  const setHovered = useSetHovered();
  const [isOpen, setIsOpen] = useState(false);

  const isPopupVisible = isOpen && popup !== undefined;

  return (
    <>
      <RMarker
        longitude={longitude}
        latitude={latitude}
        pitchAlignment="viewport"
        rotationAlignment="viewport"
        opacityWhenCovered="0.3"
        onClick={() => setIsOpen((open) => !open)}
      >
        <div
          onMouseEnter={() => setHovered({ kind, id })}
          onMouseLeave={() => setHovered(undefined)}
        >
          {children(isHovered)}
        </div>
      </RMarker>
      {isPopupVisible && (
        <RPopup
          longitude={longitude}
          latitude={latitude}
          offset={24}
          onMapClick={() => setIsOpen(false)}
        >
          {popup}
        </RPopup>
      )}
    </>
  );
}
