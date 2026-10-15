import type { PointFilterConfig, StageResult } from "@/types/Api/pipelinePreview.types";
import type { BlockStatus } from "../bench.types";
import { defaultRouteFilters } from "../state/defaults";
import { FilterListEditor } from "./FilterListEditor";
import { StageBlock } from "./StageBlock";

interface RouteBlockProps {
  status: BlockStatus;
  result: StageResult<"route"> | undefined;
  filters: PointFilterConfig[] | null | undefined;
  onFiltersChange: (filters: PointFilterConfig[] | null) => void;
  onRun: () => void;
}

export function RouteBlock({ status, result, filters, onFiltersChange, onRun }: RouteBlockProps) {
  return (
    <StageBlock title={"Route"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        kind={"point"}
        filters={filters}
        defaultFilters={defaultRouteFilters}
        onChange={onFiltersChange}
      />
    </StageBlock>
  );
}
