// Shared UI contract of the pipeline bench. FROZEN: FE-2a builds the state and blocks, FE-2b builds the views.
import type {
  ElevationProfileStageData,
  PeakMatchingStageData,
  PipelineConfig,
  RouteStageData,
  StageId,
  StageResults,
} from "@/types/Api/pipelinePreview.types";

// idle = never run, stale = config changed (or an upstream stage changed) since the last run.
export type BlockStatus = "idle" | "stale" | "running" | "done" | "error";

export interface BenchState {
  config: PipelineConfig;
  status: Record<StageId, BlockStatus>;
  results: StageResults;
  baseline: StageResults;
  raw: StageResults;
}

// Result views take the data of ONE stage for the preview config and for the baseline (null = not available).
export interface ElevationOverlayGraphProps {
  baseline?: ElevationProfileStageData | null;
  preview?: ElevationProfileStageData | null;
}

export interface RoutePreviewMapProps {
  baseline?: RouteStageData | null;
  preview?: RouteStageData | null;
}

export interface PeaksListProps {
  baseline?: PeakMatchingStageData | null;
  preview?: PeakMatchingStageData | null;
}
