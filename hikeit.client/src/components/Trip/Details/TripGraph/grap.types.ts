import type { GpxEntry } from "@/types/ApiTypes/GpxTypes";

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
