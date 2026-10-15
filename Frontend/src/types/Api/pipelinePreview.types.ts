// Mirrors Application/Trips/Analytics/Pipeline/Contracts/*.cs 1:1. Frozen contract: change via the backend first.
import type { GainDto } from "@/components/Trip/Details/TripGraph/grap.types";
import type { GpxEntry, GpxEntryWithTime } from "./gpx.types";
import type { RouteAnalytic, TimeAnalytic } from "./analytics.types";

export type StageId = "cleanup" | "analytics" | "elevationProfile" | "peakMatching" | "route";

export const allStageIds: StageId[] = [
  "cleanup",
  "analytics",
  "elevationProfile",
  "peakMatching",
  "route",
];

// Every stage depends on cleanup; there are no other edges. Mirrors StageIds.DependenciesOf.
export const stageDependencies: Record<StageId, StageId[]> = {
  cleanup: [],
  analytics: ["cleanup"],
  elevationProfile: ["cleanup"],
  peakMatching: ["cleanup"],
  route: ["cleanup"],
};

// Request: every config field is optional, undefined/null means "production default".
// Wire format of the polymorphic filter configs (same shape the existing dev preview sends).
// Point filters work on track points (cleanup, elevation profile, route).
export type PointFilterConfig =
  | { FilterType: "MedianSmoothingFilter"; value: { WindowSize: number } }
  | { FilterType: "EmaSmoothingFilter"; value: { Alpha: number } }
  | { FilterType: "MaxSpikeFilter"; value: { MaxSpike: number } }
  | { FilterType: "RoundingPrecisionFilter"; value: { Decimals: number } }
  | { FilterType: "PinLocalMaximaFilter"; value: { Enabled: boolean } }
  | { FilterType: "SimplifyFilter"; value: { ToleranceM: number } };

// Peak filters work on candidate peaks (local maxima) in peak matching only.
export type PeakFilterConfig =
  | { FilterType: "MergeNearbyByDistanceFilter"; value: { DistanceM: number } }
  | { FilterType: "MergeNearbyByElevationFilter"; value: { ElevationDeltaM: number } };

// Every stage with filters configures an ordered list; null = production default list.
export interface CleanupConfig {
  filters?: PointFilterConfig[] | null;
}

export interface ElevationProfileConfig {
  filters?: PointFilterConfig[] | null;
}

export interface PeakMatchingConfig {
  filters?: PeakFilterConfig[] | null;
  proximityRadiusM?: number | null; // DB lookup radius, not a filter
}

export interface RouteConfig {
  filters?: PointFilterConfig[] | null;
}

export interface PipelineConfig {
  cleanup?: CleanupConfig | null;
  elevationProfile?: ElevationProfileConfig | null;
  peakMatching?: PeakMatchingConfig | null;
  route?: RouteConfig | null;
}

export interface PipelinePreviewRequest {
  config: PipelineConfig;
  stages?: StageId[] | null; // null = all stages
}

// Response
export interface CleanupStageData {
  pointCountIn: number;
  pointCountOut: number;
}

export interface AnalyticsStageData {
  route: RouteAnalytic;
  time: TimeAnalytic | null;
}

export interface ElevationProfileStageData {
  start: GpxEntryWithTime;
  gains: GainDto[];
}

export interface MatchedPeak {
  peakId: number;
  name: string;
  distanceM: number;
  elevation: number;
}

export interface PeakMatchingStageData {
  candidateCount: number;
  matched: MatchedPeak[];
}

export interface RouteStageData {
  points: GpxEntry[];
}

export interface StageDataMap {
  cleanup: CleanupStageData;
  analytics: AnalyticsStageData;
  elevationProfile: ElevationProfileStageData;
  peakMatching: PeakMatchingStageData;
  route: RouteStageData;
}

export interface StageResult<S extends StageId = StageId> {
  stage: S;
  ok: boolean;
  error: string | null;
  cacheHit: boolean;
  durationMs: number;
  pointCount: number;
  payloadBytes: number;
  data: StageDataMap[S] | null;
}

export type StageResults = { [S in StageId]?: StageResult<S> };

export interface PipelinePreviewResponse {
  results: StageResults;
  baseline: StageResults;
}
