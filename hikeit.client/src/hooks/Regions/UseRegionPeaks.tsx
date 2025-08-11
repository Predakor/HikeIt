import api from "@/Utils/Api/apiRequest";
import type { RegionWithPeaks } from "@/types/ApiTypes/region.types";
import { useQuery } from "@tanstack/react-query";

export const config = {
  queryKey: (regionId: string | number) => [
    "region",
    regionId.toString(),
    "peaks",
  ],
  queryFn: (regionId: string | number) =>
    api.get<RegionWithPeaks>(`regions/${regionId}/peaks`),
  getDetailed: (regionId: string | number) =>
    api.get<RegionWithPeaks>(`regions/${regionId}/detailed`),
};

function UseRegionPeaks(regionId: number) {
  return useQuery<RegionWithPeaks>({
    queryKey: ["region", regionId, "peaks"],
    queryFn: () => config.queryFn(regionId),
    enabled: !!regionId,
  });
}

export function UseDetailedRegionPeaks(regionId: number) {
  return useQuery<RegionWithPeaks>({
    queryKey: ["region", regionId, "detailed"],
    queryFn: () => config.getDetailed(regionId),
    enabled: !!regionId,
  });
}
export default UseRegionPeaks;
