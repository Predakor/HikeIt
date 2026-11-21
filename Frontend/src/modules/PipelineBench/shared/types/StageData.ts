// Mirrors Application/Trips/Analytics/Pipeline/Contracts/StageData.cs and what StagePresenter sends per stage.
import type { GainDto } from "@/components/Trip/Details/TripGraph/grap.types";
import type { GpxEntry, GpxEntryWithTime } from "@/types/Api/gpx.types";
import type { RouteAnalytic, TimeAnalytic } from "@/types/Api/analytics.types";

export interface CleanupStageData {
  pointCountIn: number;
  pointCountOut: number;
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

// The terrain and pace stages return the computed trip analytics as they are stored.
export type TerrainStageData = RouteAnalytic;
export type PaceStageData = TimeAnalytic;

export interface StageDataMap {
  cleanup: CleanupStageData;
  terrain: TerrainStageData;
  pace: PaceStageData;
  elevationProfile: ElevationProfileStageData;
  peakMatching: PeakMatchingStageData;
  route: RouteStageData;
}
