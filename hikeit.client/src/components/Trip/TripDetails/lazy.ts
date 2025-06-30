import { lazy } from "react";

export const ELevationGraph = lazy(
  () => import("../TripDetails/TripGraph/Dev_PreviewGraph/PreviewGraphDevOnly")
);
