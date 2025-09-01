import api from "@/Utils/Api/apiRequest";
import type { PeaksAnalytics } from "@/types/Api/analytics.types";
import type { ResourceUrl } from "@/types/Api/types";
import { useQuery } from "@tanstack/react-query";

function usePeakAnalytics(resourceLink: ResourceUrl) {
  const words = resourceLink.split("/");
  const id = words[words.length - 2];

  return useQuery({
    queryKey: ["analytics", "peaks", id],
    queryFn: () => api.get<PeaksAnalytics>(resourceLink),
  });
}
export default usePeakAnalytics;
