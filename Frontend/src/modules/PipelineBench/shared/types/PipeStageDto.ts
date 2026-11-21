// Mirrors Application/Trips/Analytics/Dtos/PipeStageDto.cs.
import type { StageDataMap } from "./StageData";
import type { StageId } from "./StageId";

export interface PipeStageStepTiming {
  name: string;
  durationMs: number;
}

// Measurements of one stage run; null on a failed stage. steps (time per filter) only when requested.
export interface PipeStageTelemetry {
  durationMs: number;
  pointCount: number | null;
  payloadBytes: number;
  steps: PipeStageStepTiming[] | null;
}

export interface PipeStageResult<TStage extends StageId = StageId> {
  stage: TStage;
  ok: boolean;
  error: string | null;
  data: StageDataMap[TStage] | null;
  telemetry: PipeStageTelemetry | null;
  // The stage never ran because its input cannot support it; error holds the reason.
  skipped: boolean;
}

export type PipeStageResults = { [TStage in StageId]?: PipeStageResult<TStage> };
