import { useRef, useState } from "react";

export interface CameraOptions {
  minZoom: number;
  maxZoom: number;
  minPitch: number;
  maxPitch: number;
}

export interface CameraActions {
  setMinZoom: (value: number) => void;
  setMaxZoom: (value: number) => void;
  setMinPitch: (value: number) => void;
  setMaxPitch: (value: number) => void;
}

const DEFAULT_CAMERA_OPTIONS: CameraOptions = {
  minZoom: 12,
  maxZoom: 18,
  minPitch: 0,
  maxPitch: 60,
};

export default function useCameraControls(defaults: CameraOptions = DEFAULT_CAMERA_OPTIONS) {
  const [options, setOptions] = useState<CameraOptions>(defaults);

  const setMinZoom = (value: number) => setOptions((prev) => ({ ...prev, minZoom: value }));
  const setMaxZoom = (value: number) => setOptions((prev) => ({ ...prev, maxZoom: value }));
  const setMinPitch = (value: number) => setOptions((prev) => ({ ...prev, minPitch: value }));
  const setMaxPitch = (value: number) => setOptions((prev) => ({ ...prev, maxPitch: value }));

  const actions = useRef<CameraActions>({
    setMinZoom,
    setMaxZoom,
    setMinPitch,
    setMaxPitch,
  });

  return [options, actions.current] as const;
}
