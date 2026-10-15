import {
  allStageIds,
  type StageDataMap,
  type StageId,
  type StageResult,
  type StageResults,
} from "@/types/Api/pipelinePreview.types";
import type { TimeSpanString } from "@/types/Api/types";
import { formatValue } from "@/Utils/Formatters";

export interface StatsRow {
  label: string;
  unit: string;
  // Stage the value comes from, used to show that stage's error when the value is missing.
  stage: StageId;
  get: (results: StageResults) => number | undefined;
  // null or undefined = no good/bad direction, shown in grey.
  higherIsBetter?: boolean | null;
  format?: (value: number) => string;
}

export interface StatDelta {
  absolute: number;
  // null when the baseline is 0 (percent is undefined there).
  percent: number | null;
}

export const stageLabels: Record<StageId, string> = {
  cleanup: "Cleanup",
  analytics: "Analytics",
  elevationProfile: "Elevation profile",
  peakMatching: "Peak matching",
  route: "Route",
};

// Only successful stages expose data; a failed or missing stage yields undefined.
function okResult<S extends StageId>(
  results: StageResults,
  stage: S,
): StageResult<S> | undefined {
  const result = results[stage];
  return result?.ok ? result : undefined;
}

function okData<S extends StageId>(
  results: StageResults,
  stage: S,
): StageDataMap[S] | undefined {
  return okResult(results, stage)?.data ?? undefined;
}

// Durations arrive as "hh:mm:ss" TimeSpan strings.
export function timeSpanToSeconds(value: TimeSpanString | null | undefined): number | undefined {
  if (!value) {
    return undefined;
  }

  const [hours = 0, minutes = 0, seconds = 0] = value.split(":").map(Number);
  const total = hours * 3600 + minutes * 60 + seconds;
  return Number.isNaN(total) ? undefined : total;
}

function formatDurationSeconds(totalSeconds: number): string {
  const hours = Math.floor(totalSeconds / 3600);
  const minutes = Math.floor((totalSeconds % 3600) / 60);
  const seconds = Math.round(totalSeconds % 60);

  if (hours > 0) {
    return `${hours}h ${minutes}m`;
  }

  if (minutes > 0) {
    return `${minutes}m ${seconds}s`;
  }

  return `${seconds}s`;
}

const bytesPerKilobyte = 1024;

function bytesToKilobytes(bytes: number | undefined): number | undefined {
  return bytes === undefined ? undefined : bytes / bytesPerKilobyte;
}

const oneDecimal = (value: number) => formatValue.NumberFormatter(value, 1);
const noDecimals = (value: number) => formatValue.NumberFormatter(value, 0);

const routeRows: StatsRow[] = [
  {
    label: "Distance",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.totalDistanceMeters,
    higherIsBetter: null,
    format: oneDecimal,
  },
  {
    label: "Total ascent",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.totalAscentMeters,
    higherIsBetter: null,
    format: oneDecimal,
  },
  {
    label: "Total descent",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.totalDescentMeters,
    higherIsBetter: null,
    format: oneDecimal,
  },
  {
    label: "Highest elevation",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.highestElevationMeters,
    higherIsBetter: null,
    format: oneDecimal,
  },
  {
    label: "Lowest elevation",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.lowestElevationMeters,
    higherIsBetter: null,
    format: oneDecimal,
  },
  {
    label: "Average slope",
    unit: "%",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.averageSlopePercent,
    higherIsBetter: null,
    format: oneDecimal,
  },
  {
    label: "Average ascent slope",
    unit: "%",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.averageAscentSlopePercent,
    higherIsBetter: null,
    format: oneDecimal,
  },
  {
    label: "Average descent slope",
    unit: "%",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.averageDescentSlopePercent,
    higherIsBetter: null,
    format: oneDecimal,
  },
];

const timeRows: StatsRow[] = [
  {
    label: "Duration",
    unit: "h:mm",
    stage: "analytics",
    get: (results) => timeSpanToSeconds(okData(results, "analytics")?.time?.duration),
    higherIsBetter: false,
    format: formatDurationSeconds,
  },
  {
    label: "Active time",
    unit: "h:mm",
    stage: "analytics",
    get: (results) => timeSpanToSeconds(okData(results, "analytics")?.time?.activeTime),
    higherIsBetter: false,
    format: formatDurationSeconds,
  },
  {
    label: "Average speed",
    unit: "km/h",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.time?.averageSpeedKph,
    higherIsBetter: true,
    format: oneDecimal,
  },
];

const countRows: StatsRow[] = [
  {
    label: "Cleanup points out",
    unit: "pts",
    stage: "cleanup",
    get: (results) => okData(results, "cleanup")?.pointCountOut,
    higherIsBetter: null,
    format: noDecimals,
  },
  {
    label: "Elevation profile points",
    unit: "pts",
    stage: "elevationProfile",
    get: (results) => okData(results, "elevationProfile")?.gains.length,
    higherIsBetter: null,
    format: noDecimals,
  },
  {
    label: "Route points",
    unit: "pts",
    stage: "route",
    get: (results) => okData(results, "route")?.points.length,
    higherIsBetter: null,
    format: noDecimals,
  },
  {
    label: "Matched peaks",
    unit: "",
    stage: "peakMatching",
    get: (results) => okData(results, "peakMatching")?.matched.length,
    higherIsBetter: true,
    format: noDecimals,
  },
  {
    label: "Peak candidates",
    unit: "",
    stage: "peakMatching",
    get: (results) => okData(results, "peakMatching")?.candidateCount,
    higherIsBetter: null,
    format: noDecimals,
  },
];

// Per-stage cost rows: payload and duration are lower-is-better.
const stageCostRows: StatsRow[] = allStageIds.flatMap((stage) => [
  {
    label: "Payload size",
    unit: "KB",
    stage,
    get: (results: StageResults) => bytesToKilobytes(okResult(results, stage)?.payloadBytes),
    higherIsBetter: false,
    format: oneDecimal,
  },
  {
    label: "Compute time",
    unit: "ms",
    stage,
    get: (results: StageResults) => okResult(results, stage)?.durationMs,
    higherIsBetter: false,
    format: noDecimals,
  },
]);

export const statsRows: StatsRow[] = [
  ...routeRows,
  ...timeRows,
  ...countRows,
  ...stageCostRows,
];

export interface StatsGroup {
  stage: StageId;
  label: string;
  rows: StatsRow[];
}

// Rows grouped by the stage (config block) they come from, in pipeline order; empty groups are dropped.
export const statsGroups: StatsGroup[] = allStageIds
  .map((stage) => ({
    stage,
    label: stageLabels[stage],
    rows: statsRows.filter((row) => row.stage === stage),
  }))
  .filter((group) => group.rows.length > 0);

// Percent changes below this are treated as "unchanged" (rounding noise), so rows do not light up for nothing.
const minChangedPercent = 0.05;

// True when the value really moved: a non-zero delta, and (when a percent exists) at least minChangedPercent.
export function isChangedDelta(delta: StatDelta | null): boolean {
  if (delta === null || delta.absolute === 0) {
    return false;
  }

  return delta.percent === null || Math.abs(delta.percent) >= minChangedPercent;
}

// Error text for a stage, shown next to a missing value.
export function stageErrorOf(results: StageResults, stage: StageId): string | null {
  return results[stage]?.error ?? null;
}

// Value minus reference. Returns null when either side is missing.
export function computeDelta(
  reference: number | undefined,
  value: number | undefined,
): StatDelta | null {
  if (reference === undefined || value === undefined) {
    return null;
  }

  const absolute = value - reference;
  const hasReference = reference !== 0;
  const percent = hasReference ? (absolute / reference) * 100 : null;

  return { absolute, percent };
}
