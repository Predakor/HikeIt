import type { PipeStageResults } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import type { StageId } from "@/modules/PipelineBench/shared/types/StageId";
import type { PipelineDefaults } from "@/modules/PipelineBench/api/usePipelineDefaults";
import type { ReactNode } from "react";
import { CleanupBlock } from "../Blocks/Cleanup/CleanupBlock";
import { PaceBlock } from "../Blocks/Pace/PaceBlock";
import { PeaksBlock } from "../Blocks/Peaks/PeaksBlock";
import { ProfileBlock } from "../Blocks/Profile/ProfileBlock";
import { RouteBlock } from "../Blocks/Route/RouteBlock";
import { TerrainBlock } from "../Blocks/Terrain/TerrainBlock";
import ElevationOverlayGraph from "../ElevationOverlayGraph";
import type { usePipelineBench } from "../hooks/usePipelineBench";
import PeaksList from "../PeaksList";
import RoutePreviewMap from "../RoutePreviewMap";
import type { BenchState } from "../shared/model/bench.types";
import { StageCard } from "../StageCard/StageCard";
import { StatsTable } from "../StatsTable/StatsTable";

type Bench = ReturnType<typeof usePipelineBench>;

export interface BenchTabContext extends Pick<BenchState, "config" | "status" | "results"> {
  baseline: PipeStageResults;
  defaults: PipelineDefaults;
  updateConfig: Bench["updateConfig"];
  run: Bench["run"];
}

export interface BenchTab {
  value: string;
  label: string;
  // The tab shows this stage's status; the tab without one shows the worst of all.
  stage?: StageId;
  render: (context: BenchTabContext) => ReactNode;
}

export const benchTabs: BenchTab[] = [
  {
    value: "all",
    label: "All",
    render: ({ baseline, results }) => <StatsTable baseline={baseline} preview={results} />,
  },
  {
    value: "cleanup",
    label: "Cleanup",
    stage: "cleanup",
    render: ({ baseline, results, status, config, defaults, updateConfig, run }) => (
      <StageCard stage={"cleanup"} baseline={baseline} preview={results} fillHeight>
        <CleanupBlock
          status={status.cleanup}
          result={results.cleanup}
          filters={config.cleanup?.filters}
          defaultFilters={defaults.cleanup.filters}
          onFiltersChange={(filters) => updateConfig("cleanup", { filters })}
          onRun={() => run("cleanup")}
        />
      </StageCard>
    ),
  },
  {
    value: "terrain",
    label: "Terrain",
    stage: "terrain",
    render: ({ baseline, results, status, config, defaults, updateConfig, run }) => (
      <StageCard stage={"terrain"} baseline={baseline} preview={results} fillHeight>
        <TerrainBlock
          status={status.terrain}
          result={results.terrain}
          filters={config.terrain?.filters}
          defaultFilters={defaults.terrain.filters}
          onFiltersChange={(filters) => updateConfig("terrain", { filters })}
          onRun={() => run("terrain")}
        />
      </StageCard>
    ),
  },
  {
    value: "pace",
    label: "Pace",
    stage: "pace",
    render: ({ baseline, results, status, config, defaults, updateConfig, run }) => (
      <StageCard stage={"pace"} baseline={baseline} preview={results} fillHeight>
        <PaceBlock
          status={status.pace}
          result={results.pace}
          filters={config.pace?.filters}
          defaultFilters={defaults.pace.filters}
          onFiltersChange={(filters) => updateConfig("pace", { filters })}
          onRun={() => run("pace")}
        />
      </StageCard>
    ),
  },
  {
    value: "elevation",
    label: "Elevation",
    stage: "elevationProfile",
    render: ({ baseline, results, status, config, defaults, updateConfig, run }) => (
      <StageCard
        stage={"elevationProfile"}
        baseline={baseline}
        preview={results}
        fillHeight
        isExpandable
        belowStats={(isExpanded) => (
          <ElevationOverlayGraph
            preview={results.elevationProfile?.data ?? null}
            baseline={baseline.elevationProfile?.data ?? null}
            isExpanded={isExpanded}
          />
        )}
      >
        <ProfileBlock
          status={status.elevationProfile}
          result={results.elevationProfile}
          filters={config.elevationProfile?.filters}
          defaultFilters={defaults.elevationProfile.filters}
          onFiltersChange={(filters) => updateConfig("elevationProfile", { filters })}
          onRun={() => run("elevationProfile")}
        />
      </StageCard>
    ),
  },
  {
    value: "route",
    label: "Route",
    stage: "route",
    render: ({ baseline, results, status, config, defaults, updateConfig, run }) => (
      <StageCard
        stage={"route"}
        baseline={baseline}
        preview={results}
        fillHeight
        isExpandable
        belowStats={(isExpanded) => (
          <RoutePreviewMap
            preview={results.route?.data ?? null}
            baseline={baseline.route?.data ?? null}
            isExpanded={isExpanded}
          />
        )}
      >
        <RouteBlock
          status={status.route}
          result={results.route}
          filters={config.route?.filters}
          defaultFilters={defaults.route.filters}
          onFiltersChange={(filters) => updateConfig("route", { filters })}
          onRun={() => run("route")}
        />
      </StageCard>
    ),
  },
  {
    value: "peaks",
    label: "Peaks",
    stage: "peakMatching",
    render: ({ baseline, results, status, config, defaults, updateConfig, run }) => (
      <StageCard
        stage={"peakMatching"}
        baseline={baseline}
        preview={results}
        fillHeight
        belowStats={
          <PeaksList
            preview={results.peakMatching?.data ?? null}
            baseline={baseline.peakMatching?.data ?? null}
          />
        }
      >
        <PeaksBlock
          status={status.peakMatching}
          result={results.peakMatching}
          filters={config.peakMatching?.filters}
          defaultFilters={defaults.peakMatching.filters}
          onFiltersChange={(filters) => updateConfig("peakMatching", { filters })}
          onRun={() => run("peakMatching")}
        />
      </StageCard>
    ),
  },
];
