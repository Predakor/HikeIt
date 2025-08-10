import ManagePeaksAdminPage from "@/pages/Admin/ManagePeaksAdminPage";
import type { RouteGroup } from "../routeTypes";

export const adminRoutes: RouteGroup = {
  type: "group",
  path: "admin/",
  label: "",
  pages: [
    {
      type: "item",
      path: "",
      label: "Admin",
      Page: ManagePeaksAdminPage,
    },
    {
      type: "item",
      path: "peaks",
      label: "peaks",
      Page: ManagePeaksAdminPage,
    },
    {
      type: "item",
      path: ":regionId",
      label: "regions",
      Page: ManagePeaksAdminPage,
    },
  ],
};
