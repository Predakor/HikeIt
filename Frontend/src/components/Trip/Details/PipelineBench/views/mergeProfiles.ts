import type { ElevationProfileStageData } from "@/types/Api/pipelinePreview.types";

export interface ProfilePoint {
  dist: number;
  ele: number;
}

// null = the series has no value at this distance (outside its profile range or no data).
export interface MergedProfilePoint {
  dist: number;
  baselineEle: number | null;
  previewEle: number | null;
}

export interface MergedProfiles {
  points: MergedProfilePoint[];
  baselinePointCount: number;
  previewPointCount: number;
}

// Converts `start` + cumulative `gains` into absolute (distance, elevation) points.
export function profileToPoints(profile: ElevationProfileStageData | null | undefined): ProfilePoint[] {
  if (!profile) {
    return [];
  }

  let dist = 0;
  let ele = profile.start.ele;
  const points: ProfilePoint[] = [{ dist, ele }];

  for (const gain of profile.gains) {
    dist += gain.dist;
    ele += gain.ele;
    points.push({ dist, ele });
  }

  return points;
}

// Merges both profiles by distance: the sorted union of all distances, each series interpolated at them.
export function mergeProfiles(
  baseline: ElevationProfileStageData | null | undefined,
  preview: ElevationProfileStageData | null | undefined,
): MergedProfiles {
  const baselinePoints = profileToPoints(baseline);
  const previewPoints = profileToPoints(preview);
  const distances = unionDistances(baselinePoints, previewPoints);

  const points = distances.map((dist) => ({
    dist,
    baselineEle: elevationAt(baselinePoints, dist),
    previewEle: elevationAt(previewPoints, dist),
  }));

  return {
    points,
    baselinePointCount: baselinePoints.length,
    previewPointCount: previewPoints.length,
  };
}

function unionDistances(first: ProfilePoint[], second: ProfilePoint[]): number[] {
  const distances = new Set<number>();

  for (const point of first) {
    distances.add(point.dist);
  }

  for (const point of second) {
    distances.add(point.dist);
  }

  return Array.from(distances).sort((a, b) => a - b);
}

// Linear interpolation on a profile sorted by distance. Returns null outside the profile range.
function elevationAt(points: ProfilePoint[], dist: number): number | null {
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
function firstIndexAtOrAfter(points: ProfilePoint[], target: number): number {
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
