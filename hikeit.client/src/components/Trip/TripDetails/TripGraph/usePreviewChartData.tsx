import type { GpxEntry } from "@/types/ApiTypes/TripDtos";
import { type GainDto, type GpxGainPoint } from "./TripGraph";
import { useChartData } from "./useChartData";

export interface GpxGainPointWithPreview extends GpxGainPoint {
  previewEle?: number;
  previewSlope?: number;
  previewTime?: number;
}
export type GpxGainsData = Array<GpxGainPointWithPreview>;

export function useChartDataWithPreview(
  gains: GainDto[],
  start: GpxEntry,
  preview?: GainDto[]
) {
  const chartPoints = useChartData(gains, start);
  const previewPoints = preview ? useChartData(preview, start) : [];

  const merged = chartPoints.map((point, index) => ({
    ...point,
    previewEle: previewPoints[index]?.ele,
    previewSlope: previewPoints[index]?.slope,
    previewTime: previewPoints[index]?.time,
  }));

  return merged as GpxGainsData;
}
