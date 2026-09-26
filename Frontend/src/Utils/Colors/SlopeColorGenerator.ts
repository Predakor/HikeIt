import { haversineDistance } from "@/Utils/haversineDistance";
import type { GpxEntry } from "@/types/Api/gpx.types";

// slope % -> color, roughly modeled after common cycling/hiking climb-steepness scales
const SLOPE_COLOR_STOPS: Array<{ slope: number; color: [number, number, number] }> = [
  { slope: -20, color: [29, 78, 216] }, // steep downhill - blue
  { slope: -5, color: [34, 197, 94] }, // gentle downhill - green
  { slope: 0, color: [34, 197, 94] }, // flat - green
  { slope: 5, color: [234, 179, 8] }, // gentle climb - yellow
  { slope: 10, color: [249, 115, 22] }, // moderate climb - orange
  { slope: 20, color: [239, 68, 68] }, // steep climb - red
];

function lerp(a: number, b: number, t: number) {
  return a + (b - a) * t;
}

export function slopeToColor(slopePercent: number): string {
  const stops = SLOPE_COLOR_STOPS;

  if (slopePercent <= stops[0].slope) {
    const [r, g, b] = stops[0].color;
    return `rgb(${r}, ${g}, ${b})`;
  }

  const last = stops[stops.length - 1];
  if (slopePercent >= last.slope) {
    const [r, g, b] = last.color;
    return `rgb(${r}, ${g}, ${b})`;
  }

  for (let i = 0; i < stops.length - 1; i++) {
    const from = stops[i];
    const to = stops[i + 1];

    if (slopePercent >= from.slope && slopePercent <= to.slope) {
      const t = (slopePercent - from.slope) / (to.slope - from.slope);
      const r = Math.round(lerp(from.color[0], to.color[0], t));
      const g = Math.round(lerp(from.color[1], to.color[1], t));
      const b = Math.round(lerp(from.color[2], to.color[2], t));
      return `rgb(${r}, ${g}, ${b})`;
    }
  }

  const [r, g, b] = last.color;
  return `rgb(${r}, ${g}, ${b})`;
}

function segmentSlope(prev: GpxEntry, curr: GpxEntry, planarDistance: number): number {
  if (planarDistance <= 0) return 0;
  const eleDelta = curr.ele - prev.ele;
  return (eleDelta / planarDistance) * 100;
}

/**
 * Builds a MapLibre `line-gradient` expression that colors a route by the
 * slope (%) of each segment. Requires the layer's source to be created with
 * `lineMetrics: true`.
 */
export function buildSlopeGradientExpression(points: GpxEntry[]): unknown[] {
  const flatColor = slopeToColor(0);

  if (points.length < 2) {
    return ["interpolate", ["linear"], ["line-progress"], 0, flatColor, 1, flatColor];
  }

  const distances: number[] = [0];
  for (let i = 1; i < points.length; i++) {
    const prev = points[i - 1];
    const curr = points[i];
    distances.push(distances[i - 1] + haversineDistance(prev.lat, prev.lon, curr.lat, curr.lon));
  }

  const totalDistance = distances[distances.length - 1];
  if (totalDistance <= 0) {
    return ["interpolate", ["linear"], ["line-progress"], 0, flatColor, 1, flatColor];
  }

  const stops: Array<number | string> = [];
  const firstSlope = segmentSlope(points[0], points[1], distances[1] - distances[0]);
  stops.push(0, slopeToColor(firstSlope));

  let lastProgress = 0;
  const MIN_STEP = 1e-6;

  for (let i = 1; i < points.length; i++) {
    const segmentDistance = distances[i] - distances[i - 1];
    const slope = segmentSlope(points[i - 1], points[i], segmentDistance);
    let progress = distances[i] / totalDistance;

    if (progress <= lastProgress) {
      progress = lastProgress + MIN_STEP;
    }
    if (progress > 1) {
      progress = 1;
    }

    stops.push(progress, slopeToColor(slope));
    lastProgress = progress;
  }

  return ["interpolate", ["linear"], ["line-progress"], ...stops];
}
