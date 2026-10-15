import type { StageResults } from "@/types/Api/pipelinePreview.types";
import { formatValue } from "@/Utils/Formatters";
import { For, Stack, Table, Text } from "@chakra-ui/react";
import { Fragment, memo } from "react";
import {
  computeDelta,
  isChangedDelta,
  stageErrorOf,
  statsGroups,
  type StatDelta,
  type StatsRow,
} from "./statsRows";

interface Props {
  // The baseline is whichever run the user picked (current config or raw file); the preview is compared against it.
  baseline: StageResults;
  preview: StageResults;
}

type SourceId = "baseline" | "preview";
type DeltaTone = "neutral" | "good" | "bad" | "unknown";

const sourceIds: SourceId[] = ["baseline", "preview"];

const sourceLabels: Record<SourceId, string> = {
  baseline: "Baseline",
  preview: "Preview",
};

const reference: SourceId = "baseline";

// Fixed column widths plus no line breaks keep every row the same height, so the table never shifts when values change.
// The metric column takes the remaining width and truncates; value columns carry "1,234.5 m +4.5%".
const columns = [
  { label: "Metric", width: undefined },
  ...sourceIds.map((source) => ({ label: sourceLabels[source], width: "48" })),
];

const noWrapCell = { whiteSpace: "nowrap", fontVariantNumeric: "tabular-nums" } as const;

// "unknown" = the value changed but the row has no good/bad direction, so it gets the neutral "changed" colour.
const deltaColors: Record<DeltaTone, string> = {
  neutral: "fg.muted",
  good: "fg.success",
  bad: "fg.error",
  unknown: "fg.info",
};

// Memoised: it only depends on the results and the local reference choice, so editing a config must not re-render every row.
export const StatsTable = memo(function StatsTable({ baseline, preview }: Props) {
  const sources: Record<SourceId, StageResults> = { baseline, preview };

  return (
    <Stack gap={2}>
      <Table.Root variant={"outline"} size="sm" tableLayout={"fixed"} w={"full"}>
        <Table.Header>
          <Table.Row>
            <For each={columns}>
              {(column) => (
                <Table.ColumnHeader key={column.label} w={column.width} {...noWrapCell}>
                  {column.label}
                </Table.ColumnHeader>
              )}
            </For>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <For each={statsGroups}>
            {(group) => (
              <Fragment key={group.stage}>
                <Table.Row bg={"bg.subtle"}>
                  <Table.Cell colSpan={columns.length} fontWeight={"semibold"} fontSize={"xs"} textTransform={"uppercase"}>
                    {group.label}
                  </Table.Cell>
                </Table.Row>
                <For each={group.rows}>
                  {(row) => (
                    <StatsTableRow
                      key={`${group.stage}-${row.label}`}
                      row={row}
                      sources={sources}
                    />
                  )}
                </For>
              </Fragment>
            )}
          </For>
        </Table.Body>
      </Table.Root>
    </Stack>
  );
});

interface RowProps {
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

function StatsTableRow({ row, sources }: RowProps) {
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

interface StatCellProps {
  row: StatsRow;
  value: number | undefined;
  error: string | null;
  delta: StatDelta | null;
}

function StatCell({ row, value, error, delta }: StatCellProps) {
  // The hover title always carries the full absolute delta, even when the visible text shows a percent.
  const title = delta === null ? undefined : formatAbsoluteDelta(row, delta);

  return (
    <Table.Cell truncate title={title} {...noWrapCell}>
      <ValueText row={row} value={value} error={error} />
      {delta === null || value === undefined ? null : <DeltaText row={row} delta={delta} />}
    </Table.Cell>
  );
}

interface ValueTextProps {
  row: StatsRow;
  value: number | undefined;
  error: string | null;
}

function ValueText({ row, value, error }: ValueTextProps) {
  if (value === undefined) {
    return (
      <Text as="span" color={"fg.muted"} title={error ?? undefined}>
        {"—"}
        {error ? (
          <Text as="span" color={"fg.error"} fontSize={"xs"} ms={2}>
            {error}
          </Text>
        ) : null}
      </Text>
    );
  }

  return <Text as="span">{withUnit(formatRowValue(row, value), row.unit)}</Text>;
}

interface DeltaTextProps {
  row: StatsRow;
  delta: StatDelta;
}

function DeltaText({ row, delta }: DeltaTextProps) {
  const tone = deltaTone(delta, row.higherIsBetter);
  const text = delta.percent === null ? formatAbsoluteDelta(row, delta) : formatPercentDelta(delta.percent);

  return (
    <Text as="span" color={deltaColors[tone]} fontWeight={isChangedDelta(delta) ? "semibold" : undefined} ms={2}>
      {text}
    </Text>
  );
}

function formatRowValue(row: StatsRow, value: number): string {
  if (row.format) {
    return row.format(value);
  }

  return formatValue.NumberFormatter(value, 1);
}

function withUnit(text: string, unit: string): string {
  if (!unit) {
    return text;
  }

  return `${text} ${unit}`;
}

function signOf(value: number): string {
  if (value > 0) {
    return "+";
  }

  if (value < 0) {
    return "-";
  }

  return "";
}

function formatAbsoluteDelta(row: StatsRow, delta: StatDelta): string {
  const text = `${signOf(delta.absolute)}${formatRowValue(row, Math.abs(delta.absolute))}`;
  return withUnit(text, row.unit);
}

function formatPercentDelta(percent: number): string {
  return `${signOf(percent)}${Math.abs(percent).toFixed(1)}%`;
}

function deltaTone(delta: StatDelta, higherIsBetter: boolean | null | undefined): DeltaTone {
  const isUnchanged = !isChangedDelta(delta);
  const hasDirection = higherIsBetter === true || higherIsBetter === false;

  if (isUnchanged) {
    return "neutral";
  }

  if (!hasDirection) {
    return "unknown";
  }

  const isIncrease = delta.absolute > 0;
  const isGood = isIncrease === (higherIsBetter === true);
  return isGood ? "good" : "bad";
}
