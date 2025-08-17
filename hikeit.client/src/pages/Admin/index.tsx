import { lazyWithPreload } from "..";

export default {
  ManageRegions: lazyWithPreload(
    () => import("./Regions/ManageRegionsAdminPage")
  ),
  UpdateRegion: lazyWithPreload(
    () => import("./Regions/UpdateRegionAdminPage")
  ),
  ManagePeaks: lazyWithPreload(() => import("./ManagePeaksAdminPage")),
  AdminCenter: lazyWithPreload(() => import("./AdminCenterPage")),
};
