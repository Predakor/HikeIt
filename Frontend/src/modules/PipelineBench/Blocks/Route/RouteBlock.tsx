import type {
  PointFilterConfig,
  StageResult,
} from "@/modules/PipelineBench/api/pipelinePreview.types";
import type { BlockStatus } from "../../shared/model/bench.types";
import { ApplyToProductionButton } from "../../shared/components/ApplyToProductionButton/ApplyToProductionButton";
import * as AppSettings from "@/modules/AppSettings";
import { FilterListEditor } from "../../shared/components/FilterListEditor";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";

interface RouteBlockProps {
  status: BlockStatus;
  result: StageResult<"route"> | undefined;
  filters: PointFilterConfig[] | null | undefined;
  defaultFilters: PointFilterConfig[];
  onFiltersChange: (filters: PointFilterConfig[] | null) => void;
  onRun: () => void;
}

export function RouteBlock({
  status,
  result,
  filters,
  defaultFilters,
  onFiltersChange,
  onRun,
}: RouteBlockProps) {
  const effectiveFilters = filters ?? defaultFilters;

  return (
    <StageBlock title={"Route"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        kind={"point"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
        actions={
          <ApplyToProductionButton
            settingType={AppSettings.Type.Route}
            filters={effectiveFilters}
          />
        }
      />
    </StageBlock>
  );
}
