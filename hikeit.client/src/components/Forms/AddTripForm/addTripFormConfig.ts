import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { regionsList } from "@/data/regionsList";

export const addTripFormConfig: InputsConfig = [
  { key: "name", label: "Trip name", type: "text", min: 3, max: 63 },
  {
    key: "tripDay",
    label: "",
    type: "date",
    min: 0,
    max: Date.now(),
    required: true,
  },
  {
    key: "regionId",
    label: "Region",
    type: "select",
    collection: { items: regionsList, type: "static" },
    required: true,
  },
];
