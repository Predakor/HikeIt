import type { SelectInputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";
import { regionsList } from "@/data/regionsList";

export const regionsSchema: SelectInputConfigEntry = {
  key: "regionId",
  label: "Region",
  type: "select",
  collection: { items: regionsList, type: "static" },
  required: true,
};
