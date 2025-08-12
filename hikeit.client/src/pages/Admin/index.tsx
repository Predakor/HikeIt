import { lazy } from "react";

export default {
  ManageRegions: lazy(() => import("./Regions/ManageRegionsAdminPage")),
  UpdateRegion: lazy(() => import("./Regions/UpdateRegionAdminPage")),
  ManagePeaks: lazy(() => import("./ManagePeaksAdminPage")),
  AdminCenter: lazy(() => import("./AdminCenterPage")),
};
