import api from "@/Utils/Api/apiRequest";
import { cacheTimes } from "@/Utils/Api/staleTimes";
import type { ResourceUrl } from "@/types/Api/types";
import { useQueryClient } from "@tanstack/react-query";

export function usePrefetch() {
  const queryClient = useQueryClient();

  return (resourceLink: ResourceUrl) => {
    queryClient.prefetchQuery({
      queryFn: () => api.get(resourceLink),
      queryKey: ["resource", resourceLink],
      staleTime: cacheTimes.hour,
    });
  };
}
