import api from "@/Utils/Api/apiRequest";
import { cacheTimes } from "@/Utils/Api/staleTimes";
import type { ResourceUrl } from "@/types/Api/types";
import { useQuery } from "@tanstack/react-query";

export default function useResourceLink<T>(resourceLink: ResourceUrl) {
  resourceLink = resourceLink.replace(/^\/?api(?=\/|$)/, "");
  if (resourceLink.startsWith("/")) {
    resourceLink = resourceLink.slice(1);
  }
  return useQuery({
    queryKey: ["resource", resourceLink],
    queryFn: () => api.get<T>(resourceLink),
    staleTime: 1 || cacheTimes.hour,
  });
}
