import type { GpxEntryWithTime } from "@/types/Api/gpx.types";
import { GenerateChartData } from "../GenerateChartData";
import type { GainDto, GpxGainPoint } from "../grap.types";

// ele / previewEle are null where a series has no value at that distance (outside its range).
export interface GpxGainPointWithPreview {
  dist: number;
  ele: number | null;
  slope?: number;
  time?: number;
  previewEle?: number | null;
  previewSlope?: number;
  previewTime?: number;
  previewDist?: number;
}

export type GpxGainsData = Array<GpxGainPointWithPreview>;

// index: point N of the preview is drawn at point N of the baseline. distance: both series are put on the union of their
// distances and interpolated. auto: distance when the point counts differ (a filter dropped points), index otherwise.
export type PreviewAlignment = "auto" | "index" | "distance";

export function GenerateChartDataWithPreview(
  gains: GainDto[],
  start: GpxEntryWithTime,
  preview?: GainDto[],
  alignBy: PreviewAlignment = "auto",
): GpxGainsData {
  const chartPoints = GenerateChartData(gains, start);
  const previewPoints = preview ? GenerateChartData(preview, start) : [];

  const hasDifferentCounts =
    previewPoints.length > 0 && previewPoints.length !== chartPoints.length;
  const isAlignedByDistance = alignBy === "distance" || (alignBy === "auto" && hasDifferentCounts);

  return isAlignedByDistance ?
      alignByDistance(chartPoints, previewPoints)
    : alignByIndex(chartPoints, previewPoints);
}

function alignByIndex(chartPoints: GpxGainPoint[], previewPoints: GpxGainPoint[]): GpxGainsData {
  return chartPoints.map((point, index) => ({
    ...point,
    previewDist: previewPoints[index]?.dist,
    previewEle: previewPoints[index]?.ele,
    previewSlope: previewPoints[index]?.slope,
    previewTime: previewPoints[index]?.time,
  }));
}

function alignByDistance(chartPoints: GpxGainPoint[], previewPoints: GpxGainPoint[]): GpxGainsData {
  const distances = new Set([...chartPoints, ...previewPoints].map((point) => point.dist));

  return Array.from(distances)
    .sort((a, b) => a - b)
    .map((dist) => ({
      dist,
      ele: elevationAt(chartPoints, dist),
      previewEle: elevationAt(previewPoints, dist),
    }));
}

// Linear interpolation on a profile sorted by distance. Returns null outside the profile range.
function elevationAt(points: GpxGainPoint[], dist: number): number | null {
  const index = firstIndexAtOrAfter(points, dist);

  if (index === points.length) {
    return null;
  }

  const next = points[index];
  if (next.dist === dist) {
    return next.ele;
  }

  if (index === 0) {
    return null;
  }

  const previous = points[index - 1];
  const ratio = (dist - previous.dist) / (next.dist - previous.dist);
  return previous.ele + ratio * (next.ele - previous.ele);
}

// Binary search: first index whose distance is >= target (points.length when none).
function firstIndexAtOrAfter(points: GpxGainPoint[], target: number): number {
  let low = 0;
  let high = points.length;

  while (low < high) {
    const middle = (low + high) >> 1;

    if (points[middle].dist < target) {
      low = middle + 1;
    } else {
      high = middle;
    }
  }

  return low;
}
