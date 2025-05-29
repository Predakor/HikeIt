import apiClient from "@/Utils/Api/ApiClient";
import type { Region } from "@/data/types";
import { useQuery } from "@tanstack/react-query";

const staleTime = 3600 * 24 * 31; //cache for 31 days lol
function useRegion(id: number | undefined) {
  return useQuery({
    queryKey: ["region", id],
    queryFn: () => apiClient<Region>(`regions/${id}`),
    enabled: !!id,
    staleTime,
  });
}
export default useRegion;
