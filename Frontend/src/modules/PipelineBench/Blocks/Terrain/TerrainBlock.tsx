import * as AppSettings from "@/modules/AppSettings";
import type { TerrainFilterConfig } from "@/modules/PipelineBench/shared/types/FilterConfig";
import type { PipeStageResult } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import { ApplyToProductionButton } from "../../shared/components/ApplyToProductionButton/ApplyToProductionButton";
import { FilterListEditor } from "../../shared/components/FilterListEditor";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";
import type { BlockStatus } from "../../shared/model/bench.types";

interface TerrainBlockProps {
  status: BlockStatus;
  result: PipeStageResult<"terrain"> | undefined;
  filters: TerrainFilterConfig[] | null | undefined;
  defaultFilters: TerrainFilterConfig[];
  onFiltersChange: (filters: TerrainFilterConfig[] | null) => void;
  onRun: () => void;
}

export function TerrainBlock({
  status,
  result,
  filters,
  defaultFilters,
  onFiltersChange,
  onRun,
}: TerrainBlockProps) {
  return (
    <StageBlock title={"Terrain"} status={status} result={result} onRun={onRun}>
      <FilterListEditor
        stepTimings={result?.telemetry?.steps}
        kind={"terrain"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
        actions={
          <ApplyToProductionButton
            settingType={AppSettings.Type.Terrain}
            filters={filters ?? defaultFilters}
          />
        }
      />
    </StageBlock>
  );
}
