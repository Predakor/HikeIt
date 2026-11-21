import * as AppSettings from "@/modules/AppSettings";
import type { PaceFilterConfig } from "@/modules/PipelineBench/shared/types/FilterConfig";
import type { PipeStageResult } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import { ApplyToProductionButton } from "../../shared/components/ApplyToProductionButton/ApplyToProductionButton";
import { FilterListEditor } from "../../shared/components/FilterListEditor";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";
import type { BlockStatus } from "../../shared/model/bench.types";

interface PaceBlockProps {
  status: BlockStatus;
  result: PipeStageResult<"pace"> | undefined;
  filters: PaceFilterConfig[] | null | undefined;
  defaultFilters: PaceFilterConfig[];
  onFiltersChange: (filters: PaceFilterConfig[] | null) => void;
  onRun: () => void;
}

export function PaceBlock({
  status,
  result,
  filters,
  defaultFilters,
  onFiltersChange,
  onRun,
}: PaceBlockProps) {
  return (
    <StageBlock title={"Pace"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        stepTimings={result?.telemetry?.steps}
        kind={"pace"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
        actions={
          <ApplyToProductionButton
            settingType={AppSettings.Type.Pace}
            filters={filters ?? defaultFilters}
          />
        }
      />
    </StageBlock>
  );
}
