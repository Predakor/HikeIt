import type { StageResults } from "@/modules/PipelineBench/api/pipelinePreview.types";
import { For, Table } from "@chakra-ui/react";
import { StatCell } from "./StatCell";
import {
  computeDelta,
  isChangedDelta,
  stageErrorOf,
  type StatDelta,
  type StatsRow,
} from "./statsRows";
import { noWrapCell, reference, sourceIds, type SourceId } from "./statsTableLayout";

interface StatsTableRowProps {
  row: StatsRow;
  sources: Record<SourceId, StageResults>;
}

interface CellData {
  source: SourceId;
  value: number | undefined;
  // null for the reference column, and when either side is missing (the cell then shows only its value).
  delta: StatDelta | null;
  error: string | null;
}

export function StatsTableRow({ row, sources }: StatsTableRowProps) {
  const referenceValue = row.get(sources[reference]);
  const cells: CellData[] = sourceIds.map((source) => {
    const value = row.get(sources[source]);
    const isReference = source === reference;

    return {
      source,
      value,
      delta: isReference ? null : computeDelta(referenceValue, value),
      error: stageErrorOf(sources[source], row.stage),
    };
  });

  // A row is changed when any non-reference column moved away from the reference.
  const isChanged = cells.some((cell) => cell.delta !== null && isChangedDelta(cell.delta));

  return (
    <Table.Row bg={isChanged ? "bg.muted" : undefined}>
      <Table.Cell
        fontWeight={isChanged ? "semibold" : undefined}
        title={row.label}
        truncate
        {...noWrapCell}
      >
        {row.label}
      </Table.Cell>
      <For each={cells}>
        {(cell) => (
          <StatCell
            key={cell.source}
            row={row}
            value={cell.value}
            error={cell.error}
            delta={cell.delta}
          />
        )}
      </For>
    </Table.Row>
  );
}
