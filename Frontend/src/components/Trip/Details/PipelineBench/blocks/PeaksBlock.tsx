import type { PeakFilterConfig, StageResult } from "@/types/Api/pipelinePreview.types";
import { Flex, Input, Text } from "@chakra-ui/react";
import type { BlockStatus } from "../bench.types";
import { FilterListEditor } from "./FilterListEditor";
import { StageBlock } from "./StageBlock";

interface PeaksBlockProps {
  status: BlockStatus;
  result: StageResult<"peakMatching"> | undefined;
  filters: PeakFilterConfig[] | null | undefined;
  proximityRadiusM: number | null | undefined;
  defaultFilters: PeakFilterConfig[];
  defaultProximityRadiusM: number;
  onFiltersChange: (filters: PeakFilterConfig[] | null) => void;
  onProximityRadiusChange: (radiusM: number | null) => void;
  onRun: () => void;
}

// Peak matching is slow (DB lookup), so it only runs on its own Run button.
export function PeaksBlock({
  status,
  result,
  filters,
  proximityRadiusM,
  defaultFilters,
  defaultProximityRadiusM,
  onFiltersChange,
  onProximityRadiusChange,
  onRun,
}: PeaksBlockProps) {
  return (
    <StageBlock title={"Peak matching"} status={status} result={result} onRun={onRun} isPrimaryRun>
      <Flex align={"center"} gap={2} wrap={"wrap"}>
        <Text fontSize={"sm"}>Proximity radius (m)</Text>
        <Input
          type={"number"}
          size={"sm"}
          w={"24"}
          min={0}
          placeholder={String(defaultProximityRadiusM)}
          value={proximityRadiusM ?? ""}
          onChange={(event) => onProximityRadiusChange(parseOptionalNumber(event.target.value))}
        />
      </Flex>

      <FilterListEditor
        kind={"peak"}
        filters={filters}
        defaultFilters={defaultFilters}
        onChange={onFiltersChange}
      />
    </StageBlock>
  );
}

// Empty (or not a number) means null, which the server reads as the production radius.
function parseOptionalNumber(raw: string): number | null {
  const isEmpty = raw.trim() === "";
  const parsed = Number(raw);

  if (isEmpty || Number.isNaN(parsed)) {
    return null;
  }

  return parsed;
}
