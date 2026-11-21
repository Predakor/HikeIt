import { Stack } from "@chakra-ui/react";
import { useState } from "react";
import { useSearchParams } from "react-router";
import type { PipelineDefaults } from "./api/usePipelineDefaults";
import { BenchHeader, type BaselineSource } from "./BenchHeader/BenchHeader";
import { BenchTabs } from "./BenchTabs/BenchTabs";
import { benchTabs } from "./BenchTabs/tabsConfig";
import { usePipelineBench } from "./hooks/usePipelineBench";

const defaultView = "elevation";
const viewParamName = "tab";
const knownViews = benchTabs.map((tab) => tab.value);

interface PipelineBenchProps {
  tripId: string;
  defaults: PipelineDefaults;
}

export default function PipelineBench({ tripId, defaults }: PipelineBenchProps) {
  const { state, updateConfig, run } = usePipelineBench(tripId);
  const [searchParams, setSearchParams] = useSearchParams();
  const [baselineSource, setBaselineSource] = useState<BaselineSource>("config");

  const viewParam = searchParams.get(viewParamName);
  const isKnownView = viewParam !== null && knownViews.includes(viewParam);
  const view = isKnownView ? viewParam : defaultView;

  const setView = (next: string) => {
    const nextSearchParams = new URLSearchParams(searchParams);
    nextSearchParams.set(viewParamName, next);
    setSearchParams(nextSearchParams, { replace: true });
  };

  // What the preview is compared against: the current (DB) config run, or the untouched file.
  const baseline = baselineSource === "raw" ? state.raw : state.baseline;

  return (
    <Stack gap={6}>
      <BenchHeader
        baselineSource={baselineSource}
        onBaselineSourceChange={setBaselineSource}
        onRunAll={() => run()}
      />

      <BenchTabs
        view={view}
        context={{ ...state, baseline, defaults, updateConfig, run }}
        onViewChange={setView}
      />
    </Stack>
  );
}
