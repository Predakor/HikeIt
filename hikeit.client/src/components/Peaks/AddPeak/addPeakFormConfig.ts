import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";

export type AddPeakConfig = {
  name: string;
  height: number;
  lat: number;
  lon: number;
  regionId?: number;
};

export const addPeakFormConfig: InputsConfig = [
  {
    key: "name",
    label: "",
    type: "text",
    min: 3,
    max: 254,
    validate: (value) => {
      if (value.length < 3) return "is too short";
      if (value.length > 254) return "is too loong";
      return true;
    },
  },
  {
    key: "height",
    label: "",
    type: "number",
    min: 1,
    max: 8849,
    required: true,
  },
  {
    key: "lat",
    label: "latitude",
    type: "number",
    step: "0.001",
    min: 0,
    max: 180,
    required: true,
  },
  {
    key: "lon",
    label: "longitude",
    type: "number",
    step: "0.001",
    min: 0,
    max: 90,
    required: true,
  },
];
