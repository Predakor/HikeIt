import type { GpxEntry } from "@/types/ApiTypes/gpx.types";
import type { GainDto, GpxGainPoint } from "./grap.types";

export function GenerateChartData(gains: GainDto[], start: GpxEntry) {
  if (!gains || !start) {
    console.error("passing empty chart data");
    return [];
  }

  const chartPoints: GpxGainPoint[] = new Array(gains.length + 1);

  const first = (chartPoints[0] = {
    dist: 0,
    ele: start.ele,
    slope: 0,
    time: 0,
  });

  let dist = first.dist;
  let ele = first.ele;
  let time = first.time;

  for (let i = 0; i < gains.length; i++) {
    const curr = gains[i];

    dist += curr.dist;
    ele += curr.ele;
    time += curr?.time || 0;

    const slope = (curr.dist > 0 ? curr.ele / curr.dist : 0) * 100;
    const current: GpxGainPoint = {
      dist: Math.round(dist),
      ele: ele,
      slope: Math.abs(Math.round(slope)),
      time: time,
    };

    chartPoints[i + 1] = current;
  }

  return chartPoints;
}
