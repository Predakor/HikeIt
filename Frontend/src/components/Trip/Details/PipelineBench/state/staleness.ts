import { allStageIds, stageDependencies, type StageId } from "@/types/Api/pipelinePreview.types";
import type { BlockStatus } from "../bench.types";

type StatusMap = Record<StageId, BlockStatus>;

export function dependentsOf(stage: StageId): StageId[] {
  return allStageIds.filter((candidate) => stageDependencies[candidate].includes(stage));
}

export function affectedBy(stage: StageId): StageId[] {
  return [stage, ...dependentsOf(stage)];
}

export function markEdited(status: StatusMap, stage: StageId): StatusMap {
  const next = { ...status };

  for (const affected of affectedBy(stage)) {
    next[affected] = "stale";
  }

  return next;
}

export function staleStages(status: StatusMap, options: { includePeaks: boolean }): StageId[] {
  return allStageIds.filter((stage) => {
    const isPending = status[stage] === "idle" || status[stage] === "stale";
    const isExcludedPeak = stage === "peakMatching" && !options.includePeaks;
    const isRunnable = isPending && !isExcludedPeak;

    return isRunnable;
  });
}
