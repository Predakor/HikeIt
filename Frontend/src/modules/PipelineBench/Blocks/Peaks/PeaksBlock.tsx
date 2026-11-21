import * as AppSettings from "@/modules/AppSettings";
import type { PeakFilterConfig } from "@/modules/PipelineBench/shared/types/FilterConfig";
import type { PipeStageResult } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import { ApplyToProductionButton } from "../../shared/components/ApplyToProductionButton/ApplyToProductionButton";
import { FilterListEditor } from "../../shared/components/FilterListEditor";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";
import type { BlockStatus } from "../../shared/model/bench.types";

interface PeaksBlockProps {
  status: BlockStatus;
  result: PipeStageResult<"peakMatching"> | undefined;
  filters: PeakFilterConfig[] | null | undefined;
  defaultFilters: PeakFilterConfig[];
  onFiltersChange: (filters: PeakFilterConfig[] | null) => void;
  onRun: () => void;
}

// Peak matching is slow (DB lookup), so it only runs on its own Run button.
export function PeaksBlock({
  status,
  result,
  filters,
  defaultFilters,
  onFiltersChange,
  onRun,
}: PeaksBlockProps) {
  const effectiveFilters = filters ?? defaultFilters;

  return (
    <StageBlock title={"Peak matching"} status={status} result={result} onRun={onRun} isPrimaryRun>
      <FilterListEditor
        stepTimings={result?.telemetry?.steps}
        kind={"peak"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
        actions={
          <ApplyToProductionButton
            settingType={AppSettings.Type.PeakMatching}
            filters={effectiveFilters}
          />
        }
      />
    </StageBlock>
  );
}
