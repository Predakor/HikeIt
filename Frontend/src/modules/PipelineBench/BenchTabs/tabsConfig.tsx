import type { StageId, StageResults } from "@/modules/PipelineBench/api/pipelinePreview.types";
import type { PipelineDefaults } from "@/modules/PipelineBench/api/usePipelineDefaults";
import type { ReactNode } from "react";
import { AnalyticsBlock } from "../Blocks/Analytics/AnalyticsBlock";
import { CleanupBlock } from "../Blocks/Cleanup/CleanupBlock";
import { PeaksBlock } from "../Blocks/Peaks/PeaksBlock";
import { ProfileBlock } from "../Blocks/Profile/ProfileBlock";
import { RouteBlock } from "../Blocks/Route/RouteBlock";
import ElevationOverlayGraph from "../ElevationOverlayGraph";
import type { usePipelineBench } from "../hooks/usePipelineBench";
import PeaksList from "../PeaksList";
import RoutePreviewMap from "../RoutePreviewMap";
import type { BenchState } from "../shared/model/bench.types";
import { StageCard } from "../StageCard/StageCard";
import { StatsTable } from "../StatsTable/StatsTable";

type Bench = ReturnType<typeof usePipelineBench>;

export interface BenchTabContext extends Pick<BenchState, "config" | "status" | "results"> {
  baseline: StageResults;
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
          defaultFilters={defaults.cleanup}
          onFiltersChange={(filters) => updateConfig("cleanup", { filters })}
          onRun={() => run("cleanup")}
        />
      </StageCard>
    ),
  },
  {
    value: "analytics",
    label: "Analytics",
    stage: "analytics",
    render: ({ baseline, results, status, run }) => (
      <StageCard stage={"analytics"} baseline={baseline} preview={results}>
        <AnalyticsBlock
          status={status.analytics}
          result={results.analytics}
          onRun={() => run("analytics")}
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
          defaultFilters={defaults.elevationProfile}
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
          defaultFilters={defaults.route}
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
          defaultFilters={defaults.peaks}
          onFiltersChange={(filters) => updateConfig("peakMatching", { filters })}
          onRun={() => run("peakMatching")}
        />
      </StageCard>
    ),
  },
];
