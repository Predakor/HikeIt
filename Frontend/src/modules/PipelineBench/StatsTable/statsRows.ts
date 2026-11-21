import { GenerateChartData } from "@/components/Trip/Details/TripGraph/GenerateChartData";
import {
  allStageIds,
  type StageDataMap,
  type StageId,
  type StageResult,
  type StageResults,
} from "@/modules/PipelineBench/api/pipelinePreview.types";
import type { TimeSpanString } from "@/types/Api/types";
import type { StatAddons } from "@/types/Utils/stat.types";
import { formatSize, formatValue } from "@/Utils/Formatters";
import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";

export interface StatsRow extends Pick<StatAddons<number>, "unit" | "formatt"> {
  label: string;
  stage: StageId;
  get: (results: StageResults) => number | undefined;
  higherIsBetter?: boolean | null;
  formatt?: (value: number) => string;
}

export interface StatDelta {
  absolute: number;
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
function okResult<TStage extends StageId>(
  results: StageResults,
  stage: TStage,
): StageResult<TStage> | undefined {
  const result = results[stage];
  return result?.ok ? result : undefined;
}

function okData<TStage extends StageId>(
  results: StageResults,
  stage: TStage,
): StageDataMap[TStage] | undefined {
  return okResult(results, stage)?.data ?? undefined;
}

function timeSpanToSeconds(value: TimeSpanString | null | undefined): number | undefined {
  return value ? TimeSpan.From(value).toSeconds() : undefined;
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
    formatt: oneDecimal,
  },
  {
    label: "Total ascent",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.totalAscentMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Total descent",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.totalDescentMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Highest elevation",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.highestElevationMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Lowest elevation",
    unit: "m",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.lowestElevationMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Average slope",
    unit: "%",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.averageSlopePercent,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Average ascent slope",
    unit: "%",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.averageAscentSlopePercent,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Average descent slope",
    unit: "%",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.route.averageDescentSlopePercent,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
];

const timeRows: StatsRow[] = [
  {
    label: "Duration",
    stage: "analytics",
    get: (results) => timeSpanToSeconds(okData(results, "analytics")?.time?.duration),
    higherIsBetter: false,
    formatt: (totalSeconds: number) => TimeSpan.FromSeconds(totalSeconds).toClockString(),
  },
  {
    label: "Active time",
    stage: "analytics",
    get: (results) => timeSpanToSeconds(okData(results, "analytics")?.time?.activeTime),
    higherIsBetter: false,
    formatt: (totalSeconds: number) => TimeSpan.FromSeconds(totalSeconds).toClockString(),
  },
  {
    label: "Average speed",
    unit: "km/h",
    stage: "analytics",
    get: (results) => okData(results, "analytics")?.time?.averageSpeedKph,
    higherIsBetter: true,
    formatt: oneDecimal,
  },
];

const countRows: StatsRow[] = [
  {
    label: "Cleanup points out",
    unit: "pts",
    stage: "cleanup",
    get: (results) => okData(results, "cleanup")?.pointCountOut,
    higherIsBetter: null,
    formatt: noDecimals,
  },
  {
    label: "Elevation profile points",
    unit: "pts",
    stage: "elevationProfile",
    get: (results) => okData(results, "elevationProfile")?.gains.length,
    higherIsBetter: null,
    formatt: noDecimals,
  },
  {
    label: "Route points",
    unit: "pts",
    stage: "route",
    get: (results) => okData(results, "route")?.points.length,
    higherIsBetter: null,
    formatt: noDecimals,
  },
  {
    label: "Matched peaks",
    stage: "peakMatching",
    get: (results) => okData(results, "peakMatching")?.matched.length,
    higherIsBetter: true,
    formatt: noDecimals,
  },
  {
    label: "Peak candidates",
    stage: "peakMatching",
    get: (results) => okData(results, "peakMatching")?.candidateCount,
    higherIsBetter: null,
    formatt: noDecimals,
  },
];

// Extremes of the profile stage's own output, so they show what its filters (simplify, rounding) did to the summits.
function profileElevations(results: StageResults): number[] {
  const profile = okData(results, "elevationProfile");

  return profile ? GenerateChartData(profile.gains, profile.start).map((point) => point.ele) : [];
}

const profileRows: StatsRow[] = [
  {
    label: "Highest point",
    unit: "m",
    stage: "elevationProfile",
    get: (results) => {
      const elevations = profileElevations(results);
      return elevations.length > 0 ? Math.max(...elevations) : undefined;
    },
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Lowest point",
    unit: "m",
    stage: "elevationProfile",
    get: (results) => {
      const elevations = profileElevations(results);
      return elevations.length > 0 ? Math.min(...elevations) : undefined;
    },
    higherIsBetter: null,
    formatt: oneDecimal,
  },
];

// Per-stage cost rows: payload and duration are lower-is-better.
const stageCostRows: StatsRow[] = allStageIds.flatMap((stage) => [
  {
    label: "Payload size",
    unit: "KB",
    stage,
    get: (results: StageResults) => {
      const bytes = okResult(results, stage)?.payloadBytes;
      return bytes === undefined ? undefined : formatSize.toKilobytes(bytes);
    },
    higherIsBetter: false,
    formatt: oneDecimal,
  },
  {
    label: "Compute time",
    unit: "ms",
    stage,
    get: (results: StageResults) => okResult(results, stage)?.durationMs,
    higherIsBetter: false,
    formatt: noDecimals,
  },
]);

export const statsRows: StatsRow[] = [
  ...routeRows,
  ...timeRows,
  ...countRows,
  ...profileRows,
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
