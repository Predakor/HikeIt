import type { StageResults } from "@/types/Api/pipelinePreview.types";
import { formatValue } from "@/Utils/Formatters";
import { For, Table, Text } from "@chakra-ui/react";
import { memo } from "react";
import {
  computeDelta,
  isChangedDelta,
  stageErrorOf,
  statsRows,
  type StatDelta,
  type StatsRow,
} from "./statsRows";

interface Props {
  baseline: StageResults;
  preview: StageResults;
}

type DeltaTone = "neutral" | "good" | "bad" | "unknown";

const headers = ["Metric", "Baseline", "Preview", "Delta"];

// "unknown" = the value changed but the row has no good/bad direction, so it gets the neutral "changed" colour.
const deltaColors: Record<DeltaTone, string> = {
  neutral: "fg.muted",
  good: "fg.success",
  bad: "fg.error",
  unknown: "fg.info",
};

// Memoised: it only depends on the results, so editing a config must not re-render every row.
export const StatsTable = memo(function StatsTable({ baseline, preview }: Props) {
  return (
    <Table.Root variant={"outline"} size="sm">
      <Table.Header>
        <Table.Row>
          <For each={headers}>
            {(header) => <Table.ColumnHeader key={header}>{header}</Table.ColumnHeader>}
          </For>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        <For each={statsRows}>
          {(row) => <StatsTableRow key={row.label} row={row} baseline={baseline} preview={preview} />}
        </For>
      </Table.Body>
    </Table.Root>
  );
});

interface RowProps {
  row: StatsRow;
  baseline: StageResults;
  preview: StageResults;
}

function StatsTableRow({ row, baseline, preview }: RowProps) {
  const baselineValue = row.get(baseline);
  const previewValue = row.get(preview);
  const delta = computeDelta(baselineValue, previewValue);
  const isChanged = isChangedDelta(delta);

  return (
    <Table.Row bg={isChanged ? "bg.muted" : undefined}>
      <Table.Cell fontWeight={isChanged ? "semibold" : undefined}>{row.label}</Table.Cell>
      <Table.Cell>
        <ValueText row={row} value={baselineValue} error={stageErrorOf(baseline, row.stage)} />
      </Table.Cell>
      <Table.Cell>
        <ValueText row={row} value={previewValue} error={stageErrorOf(preview, row.stage)} />
      </Table.Cell>
      <Table.Cell>
        <DeltaText row={row} delta={delta} />
      </Table.Cell>
    </Table.Row>
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
  delta: StatDelta | null;
}

function DeltaText({ row, delta }: DeltaTextProps) {
  if (delta === null) {
    return <Text as="span" color={"fg.muted"}>{"—"}</Text>;
  }

  const tone = deltaTone(delta, row.higherIsBetter);
  return (
    <Text as="span" color={deltaColors[tone]} fontWeight={isChangedDelta(delta) ? "semibold" : undefined}>
      {formatDelta(row, delta)}
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

function formatDelta(row: StatsRow, delta: StatDelta): string {
  const absoluteText = `${signOf(delta.absolute)}${formatRowValue(row, Math.abs(delta.absolute))}`;
  const percent = delta.percent;
  if (percent === null) {
    return withUnit(absoluteText, row.unit);
  }

  const percentText = ` (${signOf(percent)}${Math.abs(percent).toFixed(1)}%)`;
  return withUnit(absoluteText, row.unit) + percentText;
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
