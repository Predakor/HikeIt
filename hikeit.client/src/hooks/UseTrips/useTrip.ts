import api from "@/Utils/Api/apiRequest";
import { cacheTimes } from "@/Utils/Api/staleTimes";
import type { TripWithBasicAnalytics } from "@/types/Api/TripDtos";
import { useQuery } from "@tanstack/react-query";
import { useEffect } from "react";
import { useParams } from "react-router";
import { usePrefetch } from "../Api/usePrefetch";

const baseKey = "trips";

export const useTripConfig = {
  queryKey: (id?: string) => [baseKey, id],
  fetchTrip: (id: string) =>
    api.get<TripWithBasicAnalytics>(`${baseKey}/${id}`),
};

export function useTrip() {
  const { tripId } = useParams();
  const prefetch = usePrefetch();

  const query = useQuery({
    queryKey: useTripConfig.queryKey(tripId),
    queryFn: () => useTripConfig.fetchTrip(tripId!),
    enabled: !!tripId,
    staleTime: cacheTimes.hour,
  });

  useEffect(() => {
    const { data, isSuccess } = query;
    if (!isSuccess || !data) {
      return;
    }

    const { peaks: peaksLink, elevation: elevationLink } = data.analytics;

    if (elevationLink) {
      prefetch(elevationLink);
    }

    if (peaksLink) {
      prefetch(peaksLink);
    }
  }, [query]);

  return query;
}
