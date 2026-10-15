import api from "@/Utils/Api/apiRequest";
import type { PeakFilterConfig, PointFilterConfig } from "@/types/Api/pipelinePreview.types";
import { useQuery } from "@tanstack/react-query";
import {
  defaultCleanupFilters,
  defaultElevationProfileFilters,
  defaultPeakFilters,
  defaultPeakProximityRadiusM,
  defaultRouteFilters,
} from "../state/defaults";

// What every stage resolves for a null config on the server (DB settings, else code constants).
export interface PipelineDefaults {
  cleanup: PointFilterConfig[];
  elevationProfile: PointFilterConfig[];
  peaks: PeakFilterConfig[];
  peakProximityRadiusM: number;
  route: PointFilterConfig[];
}

// Used while the request is loading, when it fails, and in mock mode.
const fallbackDefaults: PipelineDefaults = {
  cleanup: defaultCleanupFilters,
  elevationProfile: defaultElevationProfileFilters,
  peaks: defaultPeakFilters,
  peakProximityRadiusM: defaultPeakProximityRadiusM,
  route: defaultRouteFilters,
};

const isPipelineMockEnabled = import.meta.env.VITE_PIPELINE_MOCK === "true";

export function usePipelineDefaults(tripId?: string): PipelineDefaults {
  const { data } = useQuery({
    queryKey: ["pipeline-defaults", tripId],
    queryFn: () => api.get<PipelineDefaults>(`trips/${tripId}/analytics/pipeline/defaults`),
    enabled: !isPipelineMockEnabled && Boolean(tripId),
    staleTime: 0,
  });

  return data ?? fallbackDefaults;
}
