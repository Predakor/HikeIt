import type { PointFilterConfig, StageResult } from "@/types/Api/pipelinePreview.types";
import type { BlockStatus } from "../bench.types";
import { FilterListEditor } from "./FilterListEditor";
import { StageBlock } from "./StageBlock";

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
  return (
    <StageBlock title={"Route"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        kind={"point"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
      />
    </StageBlock>
  );
}
