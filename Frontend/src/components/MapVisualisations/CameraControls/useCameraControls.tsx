import type { InputEffectForm, InputsConfigFor } from "@/components/Utils/RenderInputs/inputTypes";
import { normalizeCamera } from "@/Utils/Maps/cameraLimits";
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

/** Pulls the partner field along when min/max would cross. */
const pullMax =
  (maxKey: keyof CameraOptions) => (value: number, form: InputEffectForm<CameraOptions>) => {
    if (value > form.getValues(maxKey)) {
      form.setValue(maxKey, value);
    }
  };
const pullMin =
  (minKey: keyof CameraOptions) => (value: number, form: InputEffectForm<CameraOptions>) => {
    if (value < form.getValues(minKey)) {
      form.setValue(minKey, value);
    }
  };

export const cameraConfig: InputsConfigFor<CameraOptions> = {
  minZoom: {
    key: "minZoom",
    label: "Min zoom",
    type: "range",
    min: 0,
    max: 24,
    step: 0.5,
    effect: pullMax("maxZoom"),
  },
  maxZoom: {
    key: "maxZoom",
    label: "Max zoom",
    type: "range",
    min: 0,
    max: 24,
    step: 0.5,
    effect: pullMin("minZoom"),
  },
  minPitch: {
    key: "minPitch",
    label: "Min pitch",
    type: "range",
    min: 0,
    max: 85,
    step: 1,
    effect: pullMax("maxPitch"),
  },
  maxPitch: {
    key: "maxPitch",
    label: "Max pitch",
    type: "range",
    min: 0,
    max: 85,
    step: 1,
    effect: pullMin("minPitch"),
  },
};

export default function useCameraControls(defaults: CameraOptions = DEFAULT_CAMERA_OPTIONS) {
  const form = useForm<CameraOptions>({ defaultValues: defaults });
  const raw = useWatch({ control: form.control });

  // the form keeps whatever the user dragged, everything downstream gets valid values
  const options = useMemo(
    () => normalizeCamera({ ...defaults, ...raw }),
    [raw.minZoom, raw.maxZoom, raw.minPitch, raw.maxPitch],
  );

  return { form, options };
}
