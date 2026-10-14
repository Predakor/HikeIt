import type { CameraOptions } from "@/components/MapVisualisations/CameraControls/useCameraControls";
import type { Map as MapLibreMap } from "maplibre-gl";

export const ZoomSpan = [0, 24] as const;
export const PitchSpan = [0, 85] as const;

const clamp = (value: number, [lo, hi]: readonly [number, number]) =>
  Math.min(Math.max(value, lo), hi);

/** Clamps both ends into the span and swaps them if they crossed. */
export function normalizeRange(
  min: number,
  max: number,
  span: readonly [number, number],
): [number, number] {
  const a = clamp(min, span);
  const b = clamp(max, span);
  return a <= b ? [a, b] : [b, a];
}

type LimitSetters = Pick<MapLibreMap, "setMinZoom" | "setMaxZoom" | "setMinPitch" | "setMaxPitch">;

/**
 * Opens a min/max pair to the full span first, then narrows it, so the order
 * never matters (the map throws if min > max at any intermediate step).
 */
function applyRange(
  setMin: (value: number) => unknown,
  setMax: (value: number) => unknown,
  [min, max]: readonly [number, number],
  [lo, hi]: readonly [number, number],
) {
  setMin(lo);
  setMax(hi);
  setMin(min);
  setMax(max);
}

/** Pushes already-normalized camera limits onto the map instance. */
export function applyCameraLimits(map: LimitSetters, options: CameraOptions) {
  applyRange(
    (v) => map.setMinZoom(v),
    (v) => map.setMaxZoom(v),
    [options.minZoom, options.maxZoom],
    ZoomSpan,
  );
  applyRange(
    (v) => map.setMinPitch(v),
    (v) => map.setMaxPitch(v),
    [options.minPitch, options.maxPitch],
    PitchSpan,
  );
}

/** Returns camera options that are always valid for the map. */
export function normalizeCamera(options: CameraOptions): CameraOptions {
  const [minZoom, maxZoom] = normalizeRange(options.minZoom, options.maxZoom, ZoomSpan);
  const [minPitch, maxPitch] = normalizeRange(options.minPitch, options.maxPitch, PitchSpan);

  return { minZoom, maxZoom, minPitch, maxPitch };
}
