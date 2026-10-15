import type { StageResults } from "@/types/Api/pipelinePreview.types";
import { formatValue } from "@/Utils/Formatters";
import { Flex, For, SegmentGroup, Stack, Table, Text } from "@chakra-ui/react";
import { Fragment, memo, useState } from "react";
import {
  computeDelta,
  isChangedDelta,
  stageErrorOf,
  statsGroups,
  type StatDelta,
  type StatsRow,
} from "./statsRows";

interface Props {
  raw: StageResults;
  baseline: StageResults;
  preview: StageResults;
}

type SourceId = "raw" | "baseline" | "preview";
type DeltaTone = "neutral" | "good" | "bad" | "unknown";

const sourceIds: SourceId[] = ["raw", "baseline", "preview"];

const sourceLabels: Record<SourceId, string> = {
  raw: "Raw",
  baseline: "Baseline",
  preview: "Preview",
};

// The reference is the column every other value is compared against. Raw is the default.
const referenceItems: { value: SourceId; label: string }[] = [
  { value: "raw", label: sourceLabels.raw },
  { value: "baseline", label: sourceLabels.baseline },
];

// Fixed column widths plus no line breaks keep every row the same height, so the table never shifts when values change.
// The metric column takes the remaining width and truncates; value columns carry "1,234.5 m +4.5%".
const columns = [
  { label: "Metric", width: undefined },
  ...sourceIds.map((source) => ({ label: sourceLabels[source], width: "44" })),
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
export const StatsTable = memo(function StatsTable({ raw, baseline, preview }: Props) {
  const [reference, setReference] = useState<SourceId>("raw");
  const sources: Record<SourceId, StageResults> = { raw, baseline, preview };

  return (
    <Stack gap={2}>
      <Flex align={"center"} gap={3} wrap={"wrap"}>
        <Text as="span" fontSize={"sm"} color={"fg.muted"}>
          Compare against:
        </Text>
        <SegmentGroup.Root
          size={"sm"}
          value={reference}
          onValueChange={(details) => {
            if (details.value === null) {
              return;
            }

            setReference(details.value as SourceId);
          }}
        >
          <SegmentGroup.Indicator />
          <SegmentGroup.Items items={referenceItems} />
        </SegmentGroup.Root>
      </Flex>

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
                      reference={reference}
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
  reference: SourceId;
  sources: Record<SourceId, StageResults>;
}

interface CellData {
  source: SourceId;
  value: number | undefined;
  // null for the reference column, and when either side is missing (the cell then shows only its value).
  delta: StatDelta | null;
  error: string | null;
}

function StatsTableRow({ row, reference, sources }: RowProps) {
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
            isReference={cell.source === reference}
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
  isReference: boolean;
}

function StatCell({ row, value, error, delta, isReference }: StatCellProps) {
  // The hover title always carries the full absolute delta, even when the visible text shows a percent.
  const title = delta === null ? undefined : formatAbsoluteDelta(row, delta);

  return (
    <Table.Cell truncate title={title} {...noWrapCell}>
      <ValueText row={row} value={value} error={error} />
      {isReference ? (
        <Text as="span" color={"fg.muted"} fontSize={"xs"} ms={2}>
          {"ref"}
        </Text>
      ) : null}
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
