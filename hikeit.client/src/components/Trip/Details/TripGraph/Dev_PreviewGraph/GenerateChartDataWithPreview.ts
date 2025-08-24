import type { GpxEntry } from "@/types/ApiTypes/gpx.types";
import { GenerateChartData } from "../GenerateChartData";
import type { GainDto, GpxGainPoint } from "../grap.types";

export interface GpxGainPointWithPreview extends GpxGainPoint {
  previewEle?: number;
  previewSlope?: number;
  previewTime?: number;
  previewDist?: number;
}

export type GpxGainsData = Array<GpxGainPointWithPreview>;

export function GenerateChartDataWithPreview(
  gains: GainDto[],
  start: GpxEntry,
  preview?: GainDto[]
) {
  const chartPoints = GenerateChartData(gains, start);
  const previewPoints = preview ? GenerateChartData(preview, start) : [];

  const merged = chartPoints.map((point, index) => ({
    ...point,
    previewDist: previewPoints[index]?.dist,
    previewEle: previewPoints[index]?.ele,
    previewSlope: previewPoints[index]?.slope,
    previewTime: previewPoints[index]?.time,
  }));

  return merged as GpxGainsData;
}
