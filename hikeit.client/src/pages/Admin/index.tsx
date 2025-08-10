import { lazy } from "react";

export default {
  ManageRegions: lazy(() => import("./ManageRegionsAdminPage")),
  ManagePeaks: lazy(() => import("./ManagePeaksAdminPage")),
  AdminCenter: lazy(() => import("./AdminCenterPage")),
};
