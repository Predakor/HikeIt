import { RegionProgression, RegionsSummaries } from "@/pages";
import type { RouteGroup } from "./route.types";

export const regionRoutes: RouteGroup = {
  type: "group",
  path: "regions",
  label: "",
  pages: [
    {
      type: "item",
      path: "",
      label: "Regions",
      Page: RegionsSummaries,
    },
    {
      type: "item",
      path: ":regionId",
      label: "Region Progression",
      Page: RegionProgression,
    },
  ],
};
