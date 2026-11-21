import api from "@/Utils/Api/apiRequest";
import type {
  PeakFilterConfig,
  PointFilterConfig,
} from "@/modules/PipelineBench/api/pipelinePreview.types";
import { useQuery } from "@tanstack/react-query";

// What every stage resolves for a null config on the server (DB settings, else code constants).
export interface PipelineDefaults {
  cleanup: PointFilterConfig[];
  elevationProfile: PointFilterConfig[];
  peaks: PeakFilterConfig[];
  route: PointFilterConfig[];
}

export function usePipelineDefaults(tripId?: string) {
  return useQuery({
    queryKey: ["pipeline-defaults", tripId],
    queryFn: () => api.get<PipelineDefaults>(`trips/${tripId}/analytics/pipeline/defaults`),
    enabled: !!tripId,
    staleTime: 0,
  });
}
