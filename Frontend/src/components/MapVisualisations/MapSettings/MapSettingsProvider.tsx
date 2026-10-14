import { useMemo, type ReactNode } from "react";
import useCameraControls from "../CameraControls/useCameraControls";
import useLayerControls from "../LayerControls/useLayerControls";
import { CameraSettingsContext, LayerFormContext, LayerOptionsContext } from "./hooks";

// owns both settings forms; children passed in stay untouched when a value changes,
// only components reading a context re-render
export function MapSettingsProvider({ children }: { children: ReactNode }) {
  const { form: cameraForm, options: cameraOptions } = useCameraControls();
  const { form: layerForm, options: layerOptions } = useLayerControls();

  const camera = useMemo(
    () => ({ form: cameraForm, options: cameraOptions }),
    [cameraForm, cameraOptions],
  );

  return (
    <CameraSettingsContext value={camera}>
      <LayerFormContext value={layerForm}>
        <LayerOptionsContext value={layerOptions}>{children}</LayerOptionsContext>
      </LayerFormContext>
    </CameraSettingsContext>
  );
}
