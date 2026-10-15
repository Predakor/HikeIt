import type { PointFilterConfig, StageResult } from "@/types/Api/pipelinePreview.types";
import type { BlockStatus } from "../bench.types";
import { defaultElevationProfileFilters } from "../state/defaults";
import { FilterListEditor } from "./FilterListEditor";
import { StageBlock } from "./StageBlock";

interface ProfileBlockProps {
  status: BlockStatus;
  result: StageResult<"elevationProfile"> | undefined;
  filters: PointFilterConfig[] | null | undefined;
  onFiltersChange: (filters: PointFilterConfig[] | null) => void;
  onRun: () => void;
}

export function ProfileBlock({ status, result, filters, onFiltersChange, onRun }: ProfileBlockProps) {
  return (
    <StageBlock title={"Elevation profile"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        kind={"point"}
        filters={filters}
        defaultFilters={defaultElevationProfileFilters}
        onChange={onFiltersChange}
      />
    </StageBlock>
  );
}
