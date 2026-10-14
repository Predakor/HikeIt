import { applyCameraLimits } from "@/Utils/Maps/cameraLimits";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { Stack } from "@chakra-ui/react";
import { useMap } from "maplibre-react-components";
import { memo, useEffect } from "react";
import type { UseFormReturn } from "react-hook-form";
import { RegionRoutesMapId } from "../MapVisualisation";
import { cameraConfig, type CameraOptions } from "./useCameraControls";

interface Props {
  form: UseFormReturn<CameraOptions>;
  options: CameraOptions;
}

function CameraControls({ form, options }: Props) {
  const map = useMap(RegionRoutesMapId);

  useEffect(() => {
    if (!map) {
      return;
    }
    applyCameraLimits(map, options);
  }, [map, options.minZoom, options.maxZoom, options.minPitch, options.maxPitch]);

  return (
    <Stack>
      <RenderInputs config={cameraConfig} formHook={form} />
    </Stack>
  );
}

export default memo(CameraControls);
