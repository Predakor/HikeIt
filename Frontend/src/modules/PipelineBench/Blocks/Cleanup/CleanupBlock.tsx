import type { PointFilterConfig } from "@/modules/PipelineBench/shared/types/FilterConfig";
import type { PipeStageResult } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import type { BlockStatus } from "../../shared/model/bench.types";
import { ApplyToProductionButton } from "../../shared/components/ApplyToProductionButton/ApplyToProductionButton";
import * as AppSettings from "@/modules/AppSettings";
import { FilterListEditor } from "../../shared/components/FilterListEditor";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";

interface CleanupBlockProps {
  status: BlockStatus;
  result: PipeStageResult<"cleanup"> | undefined;
  filters: PointFilterConfig[] | null | undefined;
  defaultFilters: PointFilterConfig[];
  onFiltersChange: (filters: PointFilterConfig[] | null) => void;
  onRun: () => void;
}

export function CleanupBlock({
  status,
  result,
  filters,
  defaultFilters,
  onFiltersChange,
  onRun,
}: CleanupBlockProps) {
  const effectiveFilters = filters ?? defaultFilters;

  return (
    <StageBlock title={"Cleanup"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        stepTimings={result?.telemetry?.steps}
        kind={"point"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
        actions={
          <ApplyToProductionButton
            settingType={AppSettings.Type.GpxDataProcessing}
            filters={effectiveFilters}
          />
        }
      />
    </StageBlock>
  );
}
