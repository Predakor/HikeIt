import { lazyWithPreload } from "../lazyWithPreload";

export default {
  ManageRegions: lazyWithPreload(() => import("./Regions/ManageRegionsAdminPage")),
  UpdateRegion: lazyWithPreload(() => import("./Regions/UpdateRegionAdminPage")),
  ManagePeaks: lazyWithPreload(() => import("./ManagePeaksAdminPage")),
  AdminCenter: lazyWithPreload(() => import("./AdminCenterPage")),
  AppSettings: lazyWithPreload(() => import("./AppSettings/AppSettigsPage")),
  PipelineBench: lazyWithPreload(() => import("./PipelineBench/PipelineBenchAdminPage")),
};
