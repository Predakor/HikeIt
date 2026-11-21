import { Stack } from "@chakra-ui/react";
import { useState } from "react";
import { useSearchParams } from "react-router";
import type { PipelineDefaults } from "./api/usePipelineDefaults";
import { BenchHeader } from "./BenchHeader/BenchHeader";
import { BenchTabs } from "./BenchTabs/BenchTabs";
import { benchTabs } from "./BenchTabs/tabsConfig";
import { usePipelineBench } from "./hooks/usePipelineBench";
import type { ComparisonTarget } from "./shared/types/PipelineConfig";

const defaultView = "elevation";
const viewParamName = "tab";
const knownViews = benchTabs.map((tab) => tab.value);

interface PipelineBenchProps {
  tripId: string;
  defaults: PipelineDefaults;
}

export default function PipelineBench({ tripId, defaults }: PipelineBenchProps) {
  const [includeSteps, setIncludeSteps] = useState(true);
  const [target, setTarget] = useState<ComparisonTarget>("currentProd");
  const { state, updateConfig, run } = usePipelineBench(tripId, includeSteps, target);
  const [searchParams, setSearchParams] = useSearchParams();

  const viewParam = searchParams.get(viewParamName);
  const isKnownView = viewParam !== null && knownViews.includes(viewParam);
  const view = isKnownView ? viewParam : defaultView;

  const setView = (next: string) => {
    const nextSearchParams = new URLSearchParams(searchParams);
    nextSearchParams.set(viewParamName, next);
    setSearchParams(nextSearchParams, { replace: true });
  };

  const baseline = state.comparisons[target];

  return (
    <Stack gap={6}>
      <BenchHeader
        target={target}
        onTargetChange={setTarget}
        includeSteps={includeSteps}
        onIncludeStepsChange={setIncludeSteps}
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
