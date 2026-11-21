import * as AppSettings from "@/modules/AppSettings";
import type {
  PointFilterConfig,
  StageResult,
} from "@/modules/PipelineBench/api/pipelinePreview.types";
import { ApplyToProductionButton } from "../../shared/components/ApplyToProductionButton/ApplyToProductionButton";
import { FilterListEditor } from "../../shared/components/FilterListEditor";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";
import type { BlockStatus } from "../../shared/model/bench.types";

interface ProfileBlockProps {
  status: BlockStatus;
  result: StageResult<"elevationProfile"> | undefined;
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
