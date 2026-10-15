import type { PointFilterConfig, StageResult } from "@/types/Api/pipelinePreview.types";
import type { BlockStatus } from "../bench.types";
import { FilterListEditor } from "./FilterListEditor";
import { StageBlock } from "./StageBlock";

interface CleanupBlockProps {
  status: BlockStatus;
  result: StageResult<"cleanup"> | undefined;
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
  return (
    <StageBlock title={"Cleanup"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        kind={"point"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
      />
    </StageBlock>
  );
}
