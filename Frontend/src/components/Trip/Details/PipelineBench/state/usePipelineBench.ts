import {
  type PipelineConfig,
  type PipelinePreviewResponse,
  type StageId,
  type StageResult,
  type StageResults,
} from "@/types/Api/pipelinePreview.types";
import { useCallback, useEffect, useRef, useState } from "react";
import { usePipelinePreview } from "../api/usePipelinePreview";
import type { BenchState, BlockStatus } from "../bench.types";
import { affectedBy, markEdited, staleStages } from "./staleness";

// Analytics has no config of its own; it is derived from cleanup.
export type ConfigStageId = Exclude<StageId, "analytics">;

const configKeyOf: Record<ConfigStageId, keyof PipelineConfig> = {
  cleanup: "cleanup",
  elevationProfile: "elevationProfile",
  peakMatching: "peakMatching",
  route: "route",
};

const autoRunDelayMs = 600;

const initialState: BenchState = {
  config: {},
  status: {
    cleanup: "idle",
    analytics: "idle",
    elevationProfile: "idle",
    peakMatching: "idle",
    route: "idle",
  },
  results: {},
  baseline: {},
};

export function usePipelineBench(tripId?: string) {
  const { mutateAsync } = usePipelinePreview(tripId);
  const [state, setState] = useState<BenchState>(initialState);

  // Refs hold the latest state for the async and debounced paths, so they never read a stale closure.
  const stateRef = useRef<BenchState>(initialState);
  // Request id that currently owns a stage. Responses only apply to stages they still own;
  // an edit releases ownership, so an in-flight result for an edited stage is dropped.
  const ownerRef = useRef<Partial<Record<StageId, number>>>({});
  const requestSeqRef = useRef(0);
  const autoRunTimerRef = useRef<ReturnType<typeof setTimeout> | undefined>(undefined);

  const commit = useCallback((next: BenchState) => {
    stateRef.current = next;
    setState(next);
  }, []);

  const runStages = useCallback(
    async (stages: StageId[]) => {
      if (stages.length === 0) {
        return;
      }

      requestSeqRef.current += 1;
      const requestId = requestSeqRef.current;
      for (const stage of stages) {
        ownerRef.current[stage] = requestId;
      }

      const requested = stateRef.current;
      const runningStatus = { ...requested.status };
      for (const stage of stages) {
        runningStatus[stage] = "running";
      }
      commit({ ...requested, status: runningStatus });

      let response: PipelinePreviewResponse | null = null;
      try {
        response = await mutateAsync({ config: requested.config, stages });
      } catch {
        response = null;
      }

      const owned = stages.filter((stage) => ownerRef.current[stage] === requestId);
      if (owned.length === 0) {
        return;
      }

      const next = stateRef.current;
      const results: Partial<Record<StageId, StageResult>> = { ...next.results };
      const baseline: Partial<Record<StageId, StageResult>> = { ...next.baseline };
      const status: Record<StageId, BlockStatus> = { ...next.status };

      for (const stage of owned) {
        delete ownerRef.current[stage];

        const result = response?.results[stage];
        if (result === undefined) {
          status[stage] = "error";
          continue;
        }

        results[stage] = result;
        baseline[stage] = response?.baseline[stage];
        status[stage] = result.ok ? "done" : "error";
      }

      // Partial records are structurally the same shape as StageResults; the cast only narrows the per-stage data type.
      commit({
        ...next,
        results: results as StageResults,
        baseline: baseline as StageResults,
        status,
      });
    },
    [mutateAsync, commit],
  );

  const scheduleAutoRun = useCallback(() => {
    clearTimeout(autoRunTimerRef.current);
    autoRunTimerRef.current = setTimeout(() => {
      void runStages(staleStages(stateRef.current.status, { includePeaks: false }));
    }, autoRunDelayMs);
  }, [runStages]);

  const updateConfig = useCallback(
    <S extends ConfigStageId>(stage: S, patch: Partial<NonNullable<PipelineConfig[S]>>) => {
      const current = stateRef.current;
      const key = configKeyOf[stage];
      const config = {
        ...current.config,
        [key]: { ...current.config[key], ...patch },
      } as PipelineConfig;

      for (const affected of affectedBy(stage)) {
        delete ownerRef.current[affected];
      }

      commit({ ...current, config, status: markEdited(current.status, stage) });
      scheduleAutoRun();
    },
    [commit, scheduleAutoRun],
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
    };
  }, []);

  const isRunning = Object.values(state.status).includes("running");

  return { state, updateConfig, run, isRunning };
}
