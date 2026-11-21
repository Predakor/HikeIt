import { formatValue } from "@/Utils/Formatters";
import { Show, Table, Text } from "@chakra-ui/react";
import { isChangedDelta, type StatDelta, type StatsRow } from "./statsRows";
import { noWrapCell } from "./statsTableLayout";

type DeltaTone = "neutral" | "good" | "bad" | "unknown";

// "unknown" = the value changed but the row has no good/bad direction, so it gets the neutral "changed" colour.
const deltaColors: Record<DeltaTone, string> = {
  neutral: "fg.muted",
  good: "fg.success",
  bad: "fg.error",
  unknown: "fg.info",
};

interface StatCellProps {
  row: StatsRow;
  value: number | undefined;
  error: string | null;
  delta: StatDelta | null;
}

export function StatCell({ row, value, error, delta }: StatCellProps) {
  // The hover title always carries the full absolute delta, even when the visible text shows a percent.
  const title = delta === null ? undefined : formatAbsoluteDelta(row, delta);
  const hasDeltaToShow = delta !== null && value !== undefined;

  return (
    <Table.Cell truncate title={title} {...noWrapCell}>
      <ValueText row={row} value={value} error={error} />
      {hasDeltaToShow && <DeltaText row={row} delta={delta} />}
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
        <Show when={error}>
          <Text as="span" color={"fg.error"} fontSize={"xs"} ms={2}>
            {error}
          </Text>
        </Show>
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
  const text =
    delta.percent === null ? formatAbsoluteDelta(row, delta) : formatPercentDelta(delta.percent);

  return (
    <Text
      as="span"
      color={deltaColors[tone]}
      fontWeight={isChangedDelta(delta) ? "semibold" : undefined}
      ms={2}
    >
      {text}
    </Text>
  );
}

function formatRowValue(row: StatsRow, value: number): string {
  if (row.formatt) {
    return row.formatt(value);
  }

  return formatValue.NumberFormatter(value, 1);
}

function withUnit(text: string, unit?: string): string {
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
