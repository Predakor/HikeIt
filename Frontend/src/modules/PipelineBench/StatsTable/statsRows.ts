import { GenerateChartData } from "@/components/Trip/Details/TripGraph/GenerateChartData";
import type { PipeStageResult, PipeStageResults } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import type { StageDataMap } from "@/modules/PipelineBench/shared/types/StageData";
import { allStageIds, type StageId } from "@/modules/PipelineBench/shared/types/StageId";
import type { TimeSpanString } from "@/types/Api/types";
import type { StatAddons } from "@/types/Utils/stat.types";
import { formatSize, formatValue } from "@/Utils/Formatters";
import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";

export interface StatsRow extends Pick<StatAddons<number>, "unit" | "formatt"> {
  label: string;
  stage: StageId;
  get: (results: PipeStageResults) => number | undefined;
  higherIsBetter?: boolean | null;
  formatt?: (value: number) => string;
}

export interface StatDelta {
  absolute: number;
  percent: number | null;
}

export const stageLabels: Record<StageId, string> = {
  cleanup: "Cleanup",
  terrain: "Terrain",
  pace: "Pace",
  elevationProfile: "Elevation profile",
  peakMatching: "Peak matching",
  route: "Route",
};

// Only successful stages expose data; a failed or missing stage yields undefined.
function okResult<TStage extends StageId>(
  results: PipeStageResults,
  stage: TStage,
): PipeStageResult<TStage> | undefined {
  const result = results[stage];
  return result?.ok ? result : undefined;
}

function okData<TStage extends StageId>(
  results: PipeStageResults,
  stage: TStage,
): StageDataMap[TStage] | undefined {
  return okResult(results, stage)?.data ?? undefined;
}

function timeSpanToSeconds(value: TimeSpanString | null | undefined): number | undefined {
  return value ? TimeSpan.From(value).toSeconds() : undefined;
}

const oneDecimal = (value: number) => formatValue.NumberFormatter(value, 1);
const noDecimals = (value: number) => formatValue.NumberFormatter(value, 0);
// Cheap stages finish in well under a millisecond, so compute time keeps the fraction.
const threeDecimals = (value: number) => formatValue.NumberFormatter(value, 3);

const routeRows: StatsRow[] = [
  {
    label: "Distance",
    unit: "m",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.totalDistanceMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Total ascent",
    unit: "m",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.totalAscentMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Total descent",
    unit: "m",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.totalDescentMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Highest elevation",
    unit: "m",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.highestElevationMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Lowest elevation",
    unit: "m",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.lowestElevationMeters,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Average slope",
    unit: "%",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.averageSlopePercent,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Average ascent slope",
    unit: "%",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.averageAscentSlopePercent,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Average descent slope",
    unit: "%",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.averageDescentSlopePercent,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Steepest climb",
    unit: "%",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.steepestClimbPercent ?? undefined,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Steepest descent",
    unit: "%",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.steepestDescentPercent ?? undefined,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Longest climb",
    unit: "m",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.longestClimbDistanceM ?? undefined,
    higherIsBetter: null,
    formatt: noDecimals,
  },
  {
    label: "Longest climb grade",
    unit: "%",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.longestClimbPercent ?? undefined,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Longest descent",
    unit: "m",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.longestDescentDistanceM ?? undefined,
    higherIsBetter: null,
    formatt: noDecimals,
  },
  {
    label: "Longest descent grade",
    unit: "%",
    stage: "terrain",
    get: (results) => okData(results, "terrain")?.longestDescentPercent ?? undefined,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
];

const timeRows: StatsRow[] = [
  {
    label: "Duration",
    stage: "pace",
    get: (results) => timeSpanToSeconds(okData(results, "pace")?.duration),
    higherIsBetter: false,
    formatt: (totalSeconds: number) => TimeSpan.FromSeconds(totalSeconds).toClockString(),
  },
  {
    label: "Active time",
    stage: "pace",
    get: (results) => timeSpanToSeconds(okData(results, "pace")?.activeTime),
    higherIsBetter: false,
    formatt: (totalSeconds: number) => TimeSpan.FromSeconds(totalSeconds).toClockString(),
  },
  {
    label: "Idle time",
    stage: "pace",
    get: (results) => timeSpanToSeconds(okData(results, "pace")?.idleTime),
    higherIsBetter: null,
    formatt: (totalSeconds: number) => TimeSpan.FromSeconds(totalSeconds).toClockString(),
  },
  {
    label: "Ascent time",
    stage: "pace",
    get: (results) => timeSpanToSeconds(okData(results, "pace")?.ascentTime),
    higherIsBetter: null,
    formatt: (totalSeconds: number) => TimeSpan.FromSeconds(totalSeconds).toClockString(),
  },
  {
    label: "Descent time",
    stage: "pace",
    get: (results) => timeSpanToSeconds(okData(results, "pace")?.descentTime),
    higherIsBetter: null,
    formatt: (totalSeconds: number) => TimeSpan.FromSeconds(totalSeconds).toClockString(),
  },
  {
    label: "Average speed",
    unit: "km/h",
    stage: "pace",
    get: (results) => okData(results, "pace")?.averageSpeedKph,
    higherIsBetter: true,
    formatt: oneDecimal,
  },
  {
    label: "Average ascent speed",
    unit: "km/h",
    stage: "pace",
    get: (results) => okData(results, "pace")?.averageAscentKph,
    higherIsBetter: true,
    formatt: oneDecimal,
  },
  {
    label: "Average descent speed",
    unit: "km/h",
    stage: "pace",
    get: (results) => okData(results, "pace")?.averageDescentKph,
    higherIsBetter: true,
    formatt: oneDecimal,
  },
  {
    label: "Fastest segment",
    unit: "km/h",
    stage: "pace",
    get: (results) => okData(results, "pace")?.fastestSpeedKph ?? undefined,
    higherIsBetter: null,
    formatt: oneDecimal,
  },
  {
    label: "Slowest segment",
    unit: "km/h",
    stage: "pace",
    get: (results) => okData(results, "pace")?.slowestSpeedKph ?? undefined,
    higherIsBetter: null,
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
function profileElevations(results: PipeStageResults): number[] {
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
    get: (results: PipeStageResults) => {
      const bytes = okResult(results, stage)?.telemetry?.payloadBytes;
      return bytes === undefined ? undefined : formatSize.toKilobytes(bytes);
    },
    higherIsBetter: false,
    formatt: oneDecimal,
  },
  {
    label: "Compute time",
    unit: "ms",
    stage,
    get: (results: PipeStageResults) => okResult(results, stage)?.telemetry?.durationMs,
    higherIsBetter: false,
    formatt: threeDecimals,
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
export function stageErrorOf(results: PipeStageResults, stage: StageId): string | null {
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
