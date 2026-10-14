import { applyCameraLimits } from "@/Utils/Maps/cameraLimits";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { useMap } from "maplibre-react-components";
import { memo, useEffect } from "react";
import type { UseFormReturn } from "react-hook-form";
import { RegionRoutesMapId } from "../MapVisualisation";
import { cameraConfig, type CameraOptions } from "./useCameraControls";

interface Props {
  form: UseFormReturn<CameraOptions>;
  options: CameraOptions;
}

const config = Object.values(cameraConfig);
function CameraControls({ form, options }: Props) {
  const map = useMap(RegionRoutesMapId);

  useEffect(() => {
    if (!map) {
      return;
    }

    // TODO: guard min/max in the form instead (a `couple` fn on range entries that moves the
    // partner field via setValue, e.g. minZoom > maxZoom pulls maxZoom up). Then options are
    // always valid and normalizeCamera in useCameraControls becomes unnecessary.
    applyCameraLimits(map, options);
  }, [map, options.minZoom, options.maxZoom, options.minPitch, options.maxPitch]);

  return <RenderInputs config={config} formHook={form} />;
}

export default memo(CameraControls);
