import type { GpxEntry } from "@/types/ApiTypes/TripDtos";
import { type GainDto, type GpxGainPoint } from "./TripGraph";

export function useChartData(gains: GainDto[], start: GpxEntry) {
  if (!gains || !start) {
    return [];
  }
  const chartPoints: GpxGainPoint[] = new Array(gains.length);

  chartPoints[0] = {
    dist: 0,
    ele: start.ele,
    slope: 0,
    time: 0,
  };

  for (let i = 1; i < chartPoints.length; i++) {
    const prev = chartPoints[i - 1];

    const dist = prev.dist + gains[i].dist;
    const ele = prev.ele + gains[i].ele;

    const current: GpxGainPoint = {
      dist: dist,
      ele: ele,
      slope: ele / dist,
      time: prev?.time || 0 + gains[i]?.time || 0,
    };

    chartPoints[i] = current;
  }
  return chartPoints;
}
