import type { GpxEntry } from "@/types/Api/gpx.types";

export interface ChartData {
  start: GpxEntry;
  gains: GainDto[];
}

export interface GpxGainPoint {
  dist: number;
  ele: number;
  slope: number;
  time?: number;
}

export type GainDto = {
  dist: number;
  ele: number;
  time: number;
};
