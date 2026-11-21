import * as AppSettings from "@/modules/AppSettings";
import type { PointFilterConfig } from "@/modules/PipelineBench/shared/types/FilterConfig";
import type { PipeStageResult } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import { ApplyToProductionButton } from "../../shared/components/ApplyToProductionButton/ApplyToProductionButton";
import { FilterListEditor } from "../../shared/components/FilterListEditor";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";
import type { BlockStatus } from "../../shared/model/bench.types";

interface ProfileBlockProps {
  status: BlockStatus;
  result: PipeStageResult<"elevationProfile"> | undefined;
  filters: PointFilterConfig[] | null | undefined;
  defaultFilters: PointFilterConfig[];
  onFiltersChange: (filters: PointFilterConfig[] | null) => void;
  onRun: () => void;
}

export function ProfileBlock({
  status,
  result,
  filters,
  defaultFilters,
  onFiltersChange,
  onRun,
}: ProfileBlockProps) {
  const effectiveFilters = filters ?? defaultFilters;

  return (
    <StageBlock title={"Elevation profile"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        stepTimings={result?.telemetry?.steps}
        kind={"point"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
        actions={
          <ApplyToProductionButton
            settingType={AppSettings.Type.ElevationProfile}
            filters={effectiveFilters}
          />
        }
      />
    </StageBlock>
  );
}
