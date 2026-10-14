import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { PitchSpan, ZoomSpan } from "@/Utils/Maps/cameraLimits";
import { useMemo } from "react";
import { useForm, useWatch } from "react-hook-form";

export interface CameraOptions {
  minZoom: number;
  maxZoom: number;
  minPitch: number;
  maxPitch: number;
}

const DEFAULT_CAMERA_OPTIONS: CameraOptions = {
  minZoom: 12,
  maxZoom: 24,
  minPitch: 0,
  maxPitch: 60,
};

// each interval drives a min/max field pair, its two thumbs can't cross
export const cameraConfig: InputsConfig = [
  {
    key: "minZoom",
    maxKey: "maxZoom",
    label: "Zoom",
    type: "interval",
    min: ZoomSpan[0],
    max: ZoomSpan[1],
    step: 0.5,
  },
  {
    key: "minPitch",
    maxKey: "maxPitch",
    label: "Pitch",
    type: "interval",
    min: PitchSpan[0],
    max: PitchSpan[1],
    step: 1,
  },
];

export default function useCameraControls(defaults: CameraOptions = DEFAULT_CAMERA_OPTIONS) {
  const form = useForm<CameraOptions>({ defaultValues: defaults });
  const watched = useWatch({ control: form.control });

  const options = useMemo(
    () => ({ ...defaults, ...watched }) as CameraOptions,
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [watched.minZoom, watched.maxZoom, watched.minPitch, watched.maxPitch],
  );

  return { form, options };
}
