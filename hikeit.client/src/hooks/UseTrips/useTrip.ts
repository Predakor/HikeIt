import api from "@/Utils/Api/apiRequest";
import { cacheTimes } from "@/Utils/Api/staleTimes";
import type { TripWithBasicAnalytics } from "@/types/ApiTypes/TripDtos";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";

const baseKey = "trips";

export const useTripConfig = {
  queryKey: (id?: string) => [baseKey, id],
  fetchTrip: (id: string) =>
    api.get<TripWithBasicAnalytics>(`${baseKey}/${id}`),
};

export function useTrip() {
  const { tripId } = useParams();

  return useQuery({
    queryKey: useTripConfig.queryKey(tripId),
    queryFn: () => useTripConfig.fetchTrip(tripId!),
    enabled: !!tripId,
    staleTime: cacheTimes.hour,
  });
}
