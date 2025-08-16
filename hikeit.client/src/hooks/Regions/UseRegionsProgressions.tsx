import apiClient from "@/Utils/Api/ApiClient";
import type {
  Region,
  RegionProgressSummary,
} from "@/types/ApiTypes/region.types";
import { useQuery, type UseQueryResult } from "@tanstack/react-query";

const staleTime = 3600 * 24 * 30;

function UseRegionsProgressions() {
  const regionsQuery = useQuery<Region[]>({
    queryKey: ["regions"],
    queryFn: () => apiClient<Region[]>("regions"),
    staleTime: staleTime,
  });

  const summariesQuery = useQuery<RegionProgressSummary[]>({
    queryKey: ["region-summaries"],
    queryFn: () => apiClient<RegionProgressSummary[]>("users/me/regions"),
  });

  const { data: regions, isSuccess: regionsLoaded } = regionsQuery;
  const { data: summaries, isSuccess: summariesLoaded } = summariesQuery;

  const allRequestsLoaded = regionsLoaded && summariesLoaded;

  const unprogressedRegions = allRequestsLoaded
    ? MergeDuplicates(regions!, summaries!)
    : [];

  return {
    data: [summaries ?? [], unprogressedRegions],
    isSuccess: allRequestsLoaded,
    isLoading: !allRequestsLoaded,
    isError: regionsQuery.isError || summariesQuery.isError,
    error: regionsQuery.error || summariesQuery.error,
  } as UseQueryResult<[RegionProgressSummary[], Region[]]>;
}

function MergeDuplicates(
  regions: Region[],
  summaries: RegionProgressSummary[]
) {
  return regions.filter(
    (region) => !summaries.some((summary) => summary.region.id === region.id)
  );
}

export default UseRegionsProgressions;
