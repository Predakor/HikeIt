import api from "@/Utils/Api/apiRequest";
import { cacheTimes } from "@/Utils/Api/staleTimes";
import type { ResourceUrl } from "@/types/ApiTypes/types";
import { useQuery } from "@tanstack/react-query";

export default function useResourceLink<T>(resourceLink: ResourceUrl) {
  return useQuery({
    queryKey: ["resource", resourceLink],
    queryFn: () => api.get<T>(resourceLink),
    staleTime: cacheTimes.hour,
  });
}
