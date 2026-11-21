// Shared UI contract of the pipeline bench. FROZEN: FE-2a builds the state and blocks, FE-2b builds the views.
import type { PipeStageResults } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import type { ComparisonTarget, PipelineConfig } from "@/modules/PipelineBench/shared/types/PipelineConfig";
import type {
  ElevationProfileStageData,
  PeakMatchingStageData,
  RouteStageData,
} from "@/modules/PipelineBench/shared/types/StageData";
import type { StageId } from "@/modules/PipelineBench/shared/types/StageId";

// idle = never run, stale = config changed (or an upstream stage changed) since the last run.
export type BlockStatus = "idle" | "stale" | "running" | "done" | "error" | "skipped";

export interface BenchState {
  config: PipelineConfig;
  status: Record<StageId, BlockStatus>;
  results: PipeStageResults;
  // What each comparison target returned so far; switching targets shows the stored ones without refetching.
  comparisons: Record<ComparisonTarget, PipeStageResults>;
}

// Result views take the data of ONE stage for the preview config and for the baseline (null = not available).
export interface ElevationOverlayGraphProps {
  baseline?: ElevationProfileStageData | null;
  preview?: ElevationProfileStageData | null;
  // Full-width row instead of the narrow column, so it is drawn taller.
  isExpanded?: boolean;
}

export interface RoutePreviewMapProps {
  baseline?: RouteStageData | null;
  preview?: RouteStageData | null;
  isExpanded?: boolean;
}

export interface PeaksListProps {
  baseline?: PeakMatchingStageData | null;
  preview?: PeakMatchingStageData | null;
}
