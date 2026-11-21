import {
  allStageIds,
  stageDependencies,
  type StageId,
} from "@/modules/PipelineBench/shared/types/StageId";
import type { BlockStatus } from "../shared/model/bench.types";

type StatusMap = Record<StageId, BlockStatus>;

export function dependentsOf(stage: StageId): StageId[] {
  return allStageIds.filter((candidate) => stageDependencies[candidate].includes(stage));
}

export function affectedBy(stage: StageId): StageId[] {
  return [stage, ...dependentsOf(stage)];
}

export function markEdited(status: StatusMap, stage: StageId): StatusMap {
  const stale = Object.fromEntries(affectedBy(stage).map((affected) => [affected, "stale"]));

  return { ...status, ...stale };
}

export function staleStages(status: StatusMap, options: { includePeaks: boolean }): StageId[] {
  return allStageIds.filter((stage) => {
    const isPending = status[stage] === "idle" || status[stage] === "stale";
    const isExcludedPeak = stage === "peakMatching" && !options.includePeaks;
    const isRunnable = isPending && !isExcludedPeak;

    return isRunnable;
  });
}
