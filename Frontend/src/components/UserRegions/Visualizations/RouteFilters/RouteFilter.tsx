import { IconColor, IconHidden, IconLocate, IconVisible } from "@/Icons/Icons";
import { getRouteBoundsAndCenter } from "@/Utils/Maps/routeBounds";
import { debounce } from "@/Utils/debounce";
import { RegionRoutesMapId } from "@/components/MapVisualisations/MapVisualisation";
import { useMapActions, useSetHovered } from "@/components/MapVisualisations/MapData/hooks";
import { DefaultWidth, type RouteData } from "@/components/MapVisualisations/MapData/mapData.types";
import { IconButton, Input, Stack } from "@chakra-ui/react";
import { useMap } from "maplibre-react-components";
import { memo, useMemo, type MouseEvent } from "react";

interface Props {
  route: RouteData;
}

function RouteFilter({ route }: Props) {
  const mapHook = useMap(RegionRoutesMapId);
  const actions = useMapActions();
  const setHovered = useSetHovered();

  const setColorDebounced = useMemo(
    () => debounce((index: number, color: string) => actions.setColor(index, color), 50),
    [actions],
  );

  const locateHandler = () => {
    const { bounds } = getRouteBoundsAndCenter([route]);
    if (!mapHook) {
      return;
    }

    mapHook.fitBounds(bounds, {
      bearing: mapHook.getBearing(),
      pitch: mapHook.getPitch(),
      linear: true,
      duration: 700,
    });
  };

  const setRouteVisibility = () => {
    actions.setVisibility(route.index, !route.visible);
  };

  const handleRowClick = (e: MouseEvent<HTMLElement>) => {
    // only the row itself, not the buttons inside it
    const isChildClick = e.target !== e.currentTarget;
    if (isChildClick) {
      return;
    }

    const calculatedWidth = route.width > DefaultWidth ? DefaultWidth : DefaultWidth * 2;
    actions.setWidth(route.index, calculatedWidth);
  };

  const handleRowDoubleClick = (e: MouseEvent<HTMLElement>) => {
    const isChildClick = e.target !== e.currentTarget;
    if (isChildClick) {
      return;
    }

    locateHandler();
  };

  return (
    <Stack
      direction={"row"}
      padding={1}
      rounded={"md"}
      cursor={"pointer"}
      bgImage={`linear-gradient(to right, ${route.color}, transparent)`}
      bgRepeat={"no-repeat"}
      bgSize={route.width > DefaultWidth ? "100% 100%" : "75% 100%"}
      transition={"background-size 0.3s ease"}
      _hover={{ bgSize: "100% 100%" }}
      onClick={handleRowClick}
      onDoubleClick={handleRowDoubleClick}
      onMouseEnter={() => setHovered({ kind: "route", id: route.index })}
      onMouseLeave={() => setHovered(undefined)}
    >
      <IconButton
        size={"sm"}
        variant={"subtle"}
        colorPalette={"green"}
        aria-label="Show/Hide route"
        onClick={setRouteVisibility}
      >
        {route.visible ?
          <IconVisible />
        : <IconHidden />}
      </IconButton>

      <IconButton
        size={"sm"}
        variant={"subtle"}
        colorPalette={"green"}
        aria-label="Locate route"
        onClick={locateHandler}
      >
        <IconLocate />
      </IconButton>

      <IconButton
        size={"sm"}
        variant={"subtle"}
        colorPalette={"green"}
        aria-label="Change route color"
        position={"relative"}
      >
        <IconColor />
        {/* invisible native picker stretched over the button, so a click opens it in place */}
        <Input
          type="color"
          position={"absolute"}
          inset={0}
          width={"full"}
          height={"full"}
          minWidth={0}
          padding={0}
          border={0}
          opacity={0}
          cursor={"pointer"}
          // disabled={slopeColoringEnabled}
          defaultValue={route.color}
          onChange={(e) => setColorDebounced(route.index, e.target.value)}
        />
      </IconButton>
    </Stack>
  );
}
export default memo(RouteFilter);
