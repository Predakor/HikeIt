import api from "@/Utils/Api/apiRequest";
import type {
  RegionWithDetailedPeaks,
  RegionWithPeaks,
} from "@/types/Api/region.types";
import { useQuery } from "@tanstack/react-query";

export const config = {
  queryKey: (regionId: string | number) => [
    "region",
    regionId.toString(),
    "peaks",
  ],

  detailed: {},

  queryFn: (regionId: string | number) =>
    api.get<RegionWithPeaks>(`regions/${regionId}/peaks`),
};

function UseRegionPeaks(regionId: number) {
  return useQuery<RegionWithPeaks>({
    queryKey: ["region", regionId, "peaks"],
    queryFn: () => config.queryFn(regionId),
    enabled: !!regionId,
  });
}

export const detailedConfig = {
  queryFn: (regionId: string | number) =>
    api.get<RegionWithDetailedPeaks>(`regions/${regionId}/detailed`),

  queryKey: (regionId: string | number) => [
    "region",
    regionId.toString(),
    "detailed",
  ],
};

export function UseDetailedRegionPeaks(regionId: number) {
  return useQuery<RegionWithDetailedPeaks>({
    queryKey: detailedConfig.queryKey(regionId),
    queryFn: () => detailedConfig.queryFn(regionId),
    enabled: !!regionId,
  });
}
export default UseRegionPeaks;
