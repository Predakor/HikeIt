import apiClient from "@/Utils/Api/ApiClient";
import type { TripDtoFull, TripSummaries } from "@/types/ApiTypes/TripDtos";
import { useQuery } from "@tanstack/react-query";

const staleTime = 1000 * 60 * 30;
export const basePath = "users/me/trips";

export const tripConfig = {
  baseKey: basePath,
  queryKey: (id?: string) => [basePath, id],
  fetchTrip: (id: string) => apiClient<TripDtoFull>(`trips/${id}/analytics`),

  fetchTrips: () => apiClient<TripSummaries>(`${basePath}/`),
};

export function useTrips() {
  return useQuery<TripSummaries>({
    queryKey: [basePath],
    queryFn: tripConfig.fetchTrips,
    staleTime: staleTime,
  });
}
