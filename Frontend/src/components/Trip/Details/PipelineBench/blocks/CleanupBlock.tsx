import type { PointFilterConfig, StageResult } from "@/types/Api/pipelinePreview.types";
import type { BlockStatus } from "../bench.types";
import { defaultCleanupFilters } from "../state/defaults";
import { FilterListEditor } from "./FilterListEditor";
import { StageBlock } from "./StageBlock";

interface CleanupBlockProps {
  status: BlockStatus;
  result: StageResult<"cleanup"> | undefined;
  filters: PointFilterConfig[] | null | undefined;
  onFiltersChange: (filters: PointFilterConfig[] | null) => void;
  onRun: () => void;
}

export function CleanupBlock({ status, result, filters, onFiltersChange, onRun }: CleanupBlockProps) {
  return (
    <StageBlock title={"Cleanup"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        kind={"point"}
        filters={filters}
        defaultFilters={defaultCleanupFilters}
        onChange={onFiltersChange}
      />
    </StageBlock>
  );
}
