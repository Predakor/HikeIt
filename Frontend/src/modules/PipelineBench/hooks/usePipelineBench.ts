import type {
  PipeStageResult,
  PipeStageResults,
} from "@/modules/PipelineBench/shared/types/PipeStageDto";
import type {
  ComparisonTarget,
  PipelineConfig,
} from "@/modules/PipelineBench/shared/types/PipelineConfig";
import { allStageIds, type StageId } from "@/modules/PipelineBench/shared/types/StageId";
import { useCallback, useEffect, useRef, useState } from "react";
import { usePipelinePreview } from "../api/usePipelinePreview";
import { affectedBy, markEdited, staleStages } from "../model/staleness";
import type { BenchState, BlockStatus } from "../shared/model/bench.types";

export type ConfigStageId = StageId;

const configKeyOf: Record<ConfigStageId, keyof PipelineConfig> = {
  cleanup: "cleanup",
  terrain: "terrain",
  pace: "pace",
  elevationProfile: "elevationProfile",
  peakMatching: "peakMatching",
  route: "route",
};

const autoRunDelayMs = 600;

interface InFlightRequest {
  stages: StageId[];
  controller: AbortController;
}

// Only the stages the response actually has; the cast narrows the per-stage data type of the entries.
const pickStages = (source: PipeStageResults, stages: StageId[]): PipeStageResults =>
  Object.fromEntries(
    stages.filter((stage) => source[stage] !== undefined).map((stage) => [stage, source[stage]]),
  ) as PipeStageResults;

const initialState: BenchState = {
  config: {},
  status: {
    cleanup: "idle",
    terrain: "idle",
    pace: "idle",
    elevationProfile: "idle",
    peakMatching: "idle",
    route: "idle",
  },
  results: {},
  comparisons: { currentProd: {}, raw: {} },
};

export function usePipelineBench(
  tripId?: string,
  includeSteps = false,
  target: ComparisonTarget = "currentProd",
) {
  const { mutateAsync } = usePipelinePreview(tripId);
  const [state, setState] = useState<BenchState>(initialState);
  const includeStepsRef = useRef(includeSteps);
  includeStepsRef.current = includeSteps;
  const targetRef = useRef(target);
  targetRef.current = target;

  // Refs hold the latest state for the async and debounced paths, so they never read a stale closure.
  const stateRef = useRef<BenchState>(initialState);
  const inFlightRef = useRef<InFlightRequest[]>([]);
  const autoRunTimerRef = useRef<ReturnType<typeof setTimeout> | undefined>(undefined);

  const commit = useCallback((next: BenchState) => {
    stateRef.current = next;
    setState(next);
  }, []);

  // Aborts every in-flight request that shares a stage with `stages`; returns all stages those requests carried.
  const cancelOverlapping = useCallback((stages: StageId[]): StageId[] => {
    const isOverlapping = (request: InFlightRequest) =>
      request.stages.some((stage) => stages.includes(stage));

    const cancelled = inFlightRef.current.filter(isOverlapping);
    cancelled.forEach((request) => request.controller.abort());
    inFlightRef.current = inFlightRef.current.filter((request) => !isOverlapping(request));

    return cancelled.flatMap((request) => request.stages);
  }, []);

  const runStages = useCallback(
    async (wanted: StageId[]) => {
      if (wanted.length === 0) {
        return;
      }

      // A superseded request takes its other stages with it, so they are re-requested together with this one.
      const stages = [...new Set([...wanted, ...cancelOverlapping(wanted)])];
      const controller = new AbortController();
      inFlightRef.current = [...inFlightRef.current, { stages, controller }];

      const requested = stateRef.current;
      const running = Object.fromEntries(stages.map((stage) => [stage, "running"]));
      commit({ ...requested, status: { ...requested.status, ...running } });

      const comparedTo = targetRef.current;

      const response = await mutateAsync({
        request: {
          config: requested.config,
          stages,
          target: comparedTo,
          includeSteps: includeStepsRef.current,
        },
        signal: controller.signal,
      }).catch((): null => null);

      if (controller.signal.aborted) {
        return;
      }

      inFlightRef.current = inFlightRef.current.filter(
        (request) => request.controller !== controller,
      );

      const next = stateRef.current;
      const received = response?.results ?? {};
      const statusOf = (result?: PipeStageResult): BlockStatus => {
        if (result?.skipped) {
          return "skipped";
        }

        return result?.ok ? "done" : "error";
      };

      commit({
        ...next,
        results: { ...next.results, ...pickStages(received, stages) },
        comparisons: {
          ...next.comparisons,
          [comparedTo]: {
            ...next.comparisons[comparedTo],
            ...pickStages(response?.comparison ?? {}, stages),
          },
        },
        status: {
          ...next.status,
          ...Object.fromEntries(stages.map((stage) => [stage, statusOf(received[stage])])),
        },
      });
    },
    [mutateAsync, commit, cancelOverlapping],
  );

  const scheduleAutoRun = useCallback(() => {
    clearTimeout(autoRunTimerRef.current);
    autoRunTimerRef.current = setTimeout(() => {
      void runStages(staleStages(stateRef.current.status, { includePeaks: false }));
    }, autoRunDelayMs);
  }, [runStages]);

  const updateConfig = useCallback(
    <TStage extends ConfigStageId>(
      stage: TStage,
      patch: Partial<NonNullable<PipelineConfig[TStage]>>,
    ) => {
      const current = stateRef.current;
      const key = configKeyOf[stage];
      const config = {
        ...current.config,
        [key]: { ...current.config[key], ...patch },
      } as PipelineConfig;

      // Cancelled requests also carried other stages; those go back to stale so the auto-run re-requests them.
      const cancelledStages = cancelOverlapping(affectedBy(stage));
      const staleAgain = Object.fromEntries(
        cancelledStages.map((cancelled) => [cancelled, "stale"]),
      );

      commit({
        ...current,
        config,
        status: { ...markEdited(current.status, stage), ...staleAgain },
      });
      scheduleAutoRun();
    },
    [commit, scheduleAutoRun, cancelOverlapping],
  );

  const run = useCallback(
    (stage?: StageId) => {
      const stages =
        stage === undefined ?
          staleStages(stateRef.current.status, { includePeaks: true })
        : [stage];

      return runStages(stages);
    },
    [runStages],
  );

  // Mount only: all blocks start idle, so this requests every pure stage once.
  useEffect(() => {
    scheduleAutoRun();

    return () => {
      clearTimeout(autoRunTimerRef.current);
      inFlightRef.current.forEach((request) => request.controller.abort());
    };
  }, []);

  // A newly picked target has no comparison yet; fetch it for the stages it lacks.
  const previousTargetRef = useRef(target);
  useEffect(() => {
    if (previousTargetRef.current === target) {
      return;
    }

    previousTargetRef.current = target;
    const missing = allStageIds.filter(
      (stage) =>
        stage !== "peakMatching" && stateRef.current.comparisons[target][stage] === undefined,
    );

    void runStages(missing);
  }, [target, runStages]);

  const isRunning = Object.values(state.status).includes("running");

  return { state, updateConfig, run, isRunning };
}
