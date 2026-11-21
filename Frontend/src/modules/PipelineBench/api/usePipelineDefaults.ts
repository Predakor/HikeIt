import api from "@/Utils/Api/apiRequest";
import type {
  CleanupConfig,
  PaceConfig,
  PeaksConfig,
  ProfileConfig,
  RouteConfig,
  TerrainConfig,
} from "@/modules/PipelineBench/shared/types/PipelineConfig";
import { useQuery } from "@tanstack/react-query";

type Resolved<TConfig> = { [TKey in keyof TConfig]-?: NonNullable<TConfig[TKey]> };

// Same shape as PipelineConfig, but every stage and list is filled with what a null config resolves to on the server.
export interface PipelineDefaults {
  cleanup: Resolved<CleanupConfig>;
  elevationProfile: Resolved<ProfileConfig>;
  peakMatching: Resolved<PeaksConfig>;
  route: Resolved<RouteConfig>;
  terrain: Resolved<TerrainConfig>;
  pace: Resolved<PaceConfig>;
}

export function usePipelineDefaults(tripId?: string) {
  return useQuery({
    queryKey: ["pipeline-defaults", tripId],
    queryFn: () => api.get<PipelineDefaults>(`trips/${tripId}/analytics/pipeline/defaults`),
    enabled: !!tripId,
    staleTime: 0,
  });
}
