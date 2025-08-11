import apiClient from "@/Utils/Api/ApiClient";
import type { Region } from "@/types/ApiTypes/region.types";
import { useQuery } from "@tanstack/react-query";

const staleTime = 1000 * 60 * 60 * 24; //cache for 1 day
function useRegion(regionId: number | undefined) {
  return useQuery({
    queryKey: ["region", regionId],
    queryFn: () => apiClient<Region>(`regions/${regionId}`),
    enabled: !!regionId,
    staleTime,
  });
}
export default useRegion;
