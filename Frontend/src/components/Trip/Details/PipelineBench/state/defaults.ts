import type { PeakFilterConfig, PointFilterConfig } from "@/types/Api/pipelinePreview.types";

// Mirror of the production defaults in Application/Trips/Analytics/Pipeline/PipelineDefaults.cs.
// A null field in the request resolves to these on the server; keep them in sync when those change.

export const defaultCleanupFilters: PointFilterConfig[] = [
  { FilterType: "MaxSpikeFilter", value: { MaxSpike: 8 } },
  { FilterType: "MedianSmoothingFilter", value: { WindowSize: 6 } },
  { FilterType: "EmaSmoothingFilter", value: { Alpha: 0.88 } },
  { FilterType: "RoundingPrecisionFilter", value: { Decimals: 1 } },
];

export const defaultElevationProfileFilters: PointFilterConfig[] = [];

export const defaultPeakFilters: PeakFilterConfig[] = [
  { FilterType: "MergeNearbyByDistanceFilter", value: { DistanceM: 50 } },
  { FilterType: "MergeNearbyByElevationFilter", value: { ElevationDeltaM: 50 } },
];

export const defaultPeakProximityRadiusM = 100;

// Route filters are empty: the route stage then uses the server setting.
export const defaultRouteFilters: PointFilterConfig[] = [];
