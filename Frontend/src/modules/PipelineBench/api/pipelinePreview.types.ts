// Mirrors Application/Trips/Analytics/Pipeline/Contracts/*.cs 1:1. Frozen contract: change via the backend first.
import type { GainDto } from "@/components/Trip/Details/TripGraph/grap.types";
import type { GpxEntry, GpxEntryWithTime } from "../../../types/Api/gpx.types";
import type { RouteAnalytic, TimeAnalytic } from "../../../types/Api/analytics.types";

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

/** Server flags enum as comma-separated names ("Latitude, Elevation") or an alias ("Coordinates", "All"). Omitted means "Elevation". */
export type PointFields = string;

// Request: every config field is optional, undefined/null means "production default".
// Point filters work on track points (cleanup, elevation profile, route).
export type PointFilterConfig =
  | { filterType: "MedianSmoothingFilter"; windowSize: number; targets?: PointFields }
  | { filterType: "EmaSmoothingFilter"; alpha: number; targets?: PointFields }
  | { filterType: "MaxSpikeFilter"; maxSpike: number }
  | { filterType: "RoundingPrecisionFilter"; decimals: number; targets?: PointFields }
  | { filterType: "PinLocalMaximaFilter"; enabled: boolean }
  | { filterType: "SimplifyFilter"; toleranceM: number };

export type PeakFilterConfig =
  | { filterType: "LocalMaximaFilter"; minClimbM: number }
  | { filterType: "MergeNearbyByDistanceFilter"; distanceM: number }
  | { filterType: "MergeNearbyByElevationFilter"; elevationDeltaM: number }
  | { filterType: "PeakLookupFilter"; offsetM: number }
  | { filterType: "MatchWithinDistanceFilter"; maxDistanceM: number };

// Every stage with filters configures an ordered list; null = production default list.
export interface CleanupConfig {
  filters?: PointFilterConfig[] | null;
}

export interface ElevationProfileConfig {
  filters?: PointFilterConfig[] | null;
}

export interface PeakMatchingConfig {
  filters?: PeakFilterConfig[] | null;
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

export interface StageResult<TStage extends StageId = StageId> {
  stage: TStage;
  ok: boolean;
  error: string | null;
  cacheHit: boolean;
  durationMs: number;
  pointCount: number;
  payloadBytes: number;
  data: StageDataMap[TStage] | null;
}

export type StageResults = { [TStage in StageId]?: StageResult<TStage> };
