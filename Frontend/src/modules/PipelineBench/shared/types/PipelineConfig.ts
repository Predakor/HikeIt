// Mirrors Application/Trips/Analytics/Pipeline/Contracts/PipelineConfig.cs.
// Request: every config field is optional, undefined/null means "production default".
import type {
  PaceFilterConfig,
  PeakFilterConfig,
  PointFilterConfig,
  TerrainFilterConfig,
} from "./FilterConfig";

// Every stage with filters configures an ordered list; null = production default list.
export interface CleanupConfig {
  filters?: PointFilterConfig[] | null;
}

export interface ProfileConfig {
  filters?: PointFilterConfig[] | null;
}

export interface PeaksConfig {
  filters?: PeakFilterConfig[] | null;
}

export interface RouteConfig {
  filters?: PointFilterConfig[] | null;
}

export interface TerrainConfig {
  filters?: TerrainFilterConfig[] | null;
}

export interface PaceConfig {
  filters?: PaceFilterConfig[] | null;
}

export interface PipelineConfig {
  cleanup?: CleanupConfig | null;
  terrain?: TerrainConfig | null;
  pace?: PaceConfig | null;
  elevationProfile?: ProfileConfig | null;
  peakMatching?: PeaksConfig | null;
  route?: RouteConfig | null;
}

// What the preview is compared against: the production settings, or the untouched file (cleanup with no filters).
export type ComparisonTarget = "currentProd" | "raw";
