import type { InputsLayout } from "@/components/Utils/RenderInputs/inputTypes";
import { useForm, useWatch } from "react-hook-form";

export interface LayerOptions {
  hillshade: boolean;
  terrain: boolean;
  exaggeration: number;
}

const DEFAULT_LAYER_OPTIONS: LayerOptions = {
  hillshade: true,
  terrain: true,
  exaggeration: 1,
};

export const layerConfig: InputsLayout = [
  {
    type: "group",
    key: "toggles",
    direction: "row",
    items: [
      { key: "hillshade", label: "Hillshade", type: "checkbox" },
      { key: "terrain", label: "3D terrain", type: "checkbox" },
    ],
  },
  {
    key: "exaggeration",
    label: "Terrain exaggeration",
    type: "range",
    min: 0.5,
    max: 5,
    step: 0.1,
  },
];

export default function useLayerControls(defaults: LayerOptions = DEFAULT_LAYER_OPTIONS) {
  const form = useForm<LayerOptions>({ defaultValues: defaults });
  const watched = useWatch({ control: form.control });

  return { form, options: { ...defaults, ...watched } as LayerOptions };
}
