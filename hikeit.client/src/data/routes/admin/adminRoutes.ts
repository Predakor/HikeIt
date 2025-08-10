import type { RouteGroup } from "../routeTypes";
import AdminPages from "@/pages/Admin";
export const adminRoutes: RouteGroup = {
  type: "group",
  path: "admin/",
  label: "",
  pages: [
    {
      type: "item",
      path: "",
      label: "Admin",
      Page: AdminPages.AdminCenter,
    },
    {
      type: "item",
      path: "peaks",
      label: "peaks",
      Page: AdminPages.ManagePeaks,
    },
    {
      type: "item",
      path: "regions",
      label: "regions",
      Page: AdminPages.ManageRegions,
    },
  ],
};
