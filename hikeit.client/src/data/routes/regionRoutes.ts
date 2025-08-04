import { RegionProgression, RegionsSummaries } from "@/pages/__index";
import type { RouteGroup } from "./routeTypes";

export const regionRoutes: RouteGroup = {
  type: "group",
  path: "regions/",
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
