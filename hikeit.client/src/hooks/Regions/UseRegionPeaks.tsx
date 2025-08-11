import api from "@/Utils/Api/apiRequest";
import type { RegionWithPeaks } from "@/types/ApiTypes/types";
import { useQuery } from "@tanstack/react-query";

function UseRegionPeaks(regionId: number) {
  const request = useQuery<RegionWithPeaks>({
    queryKey: ["region", regionId, "peaks"],
    queryFn: () => api.get<RegionWithPeaks>(`regions/${regionId}/peaks`),
    enabled: !!regionId,
  });

  return request;
}
export default UseRegionPeaks;
