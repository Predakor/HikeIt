import api from "@/Utils/Api/apiRequest";
import type { RegionWithPeaks } from "@/types/ApiTypes/types";
import { useQuery } from "@tanstack/react-query";

export const config = {
  queryKey: (regionId: string | number) => [
    "region",
    regionId.toString(),
    "peaks",
  ],
  queryFn: (regionId: string | number) =>
    api.get<RegionWithPeaks>(`regions/${regionId}/peaks`),
};

function UseRegionPeaks(regionId: number) {
  const request = useQuery<RegionWithPeaks>({
    queryKey: ["region", regionId, "peaks"],
    queryFn: () => config.queryFn(regionId),
    enabled: !!regionId,
  });

  return request;
}
export default UseRegionPeaks;
