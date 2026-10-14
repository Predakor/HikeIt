import { applyCameraLimits } from "@/Utils/Maps/cameraLimits";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { Stack } from "@chakra-ui/react";
import { useMap } from "maplibre-react-components";
import { memo, useEffect } from "react";
import { useCameraSettings } from "../MapSettings/hooks";
import { RegionRoutesMapId } from "../MapVisualisation";
import { cameraConfig } from "./useCameraControls";

function CameraControls() {
  const { form, options } = useCameraSettings();
  const map = useMap(RegionRoutesMapId);

  useEffect(() => {
    if (!map) {
      return;
    }
    applyCameraLimits(map, options);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [map, options.minZoom, options.maxZoom, options.minPitch, options.maxPitch]);

  return (
    <Stack>
      <RenderInputs config={cameraConfig} formHook={form} />
    </Stack>
  );
}

export default memo(CameraControls);
