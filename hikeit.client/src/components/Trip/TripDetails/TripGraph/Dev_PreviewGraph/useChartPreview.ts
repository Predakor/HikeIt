import api from "@/Utils/Api/apiRequest";
import type { ChartData } from "../grap.types";
import type { ElevationProfileConfig } from "./DevPreview";

function useChartPreview(id: any, onSucces: (data: ChartData) => void) {
  const sendRequest = async (data: ElevationProfileConfig) => {
    const payload = {
      MaxElevationSpike: data?.MaxElevationSpike || null,
      EmaSmoothingAlpha: data?.EmaSmoothingAlpha || null,
      MedianFilterWindowSize: data?.MedianFilterWindowSize || null,
      RoundingDecimalsCount: data?.RoundingDecimalsCount || null,
      DownsamplingFactor: data?.DownsamplingFactor || null,
    };

    const request = await api.post<ChartData>(
      `trips/${id}/analytics/elevations/preview`,
      payload
    );

    if (request) {
      onSucces(request);
    }
  };

  return sendRequest;
}
export default useChartPreview;
