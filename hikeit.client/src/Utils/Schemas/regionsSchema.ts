import type { SelectInputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";
import { regionsList } from "@/data/regionsList";
import type { CollectionItem } from "@chakra-ui/react";

export const regionsSchema: SelectInputConfigEntry = {
  key: "regionId",
  label: "Region",
  type: "select",
  collection: { items: regionsList as CollectionItem[], type: "static" },
  required: true,
};
