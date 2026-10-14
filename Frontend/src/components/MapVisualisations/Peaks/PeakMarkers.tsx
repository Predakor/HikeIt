import { memo } from "react";
import { useLayerOptions } from "../MapSettings/hooks";
import PeakMarker from "./PeakMarker";
import { usePeakMarkers } from "./usePeakMarkers";

function PeakMarkers() {
  // hooks run before the early return so the toggle does not change hook order
  const peaks = usePeakMarkers();
  const { peaks: showPeaks } = useLayerOptions();

  if (!showPeaks) {
    return null;
  }

  return (
    <>
      {peaks.map((peak) => (
        <PeakMarker key={peak.id} peak={peak} />
      ))}
    </>
  );
}

export default memo(PeakMarkers);
