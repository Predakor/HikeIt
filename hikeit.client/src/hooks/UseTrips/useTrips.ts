import apiClient from "@/Utils/Api/ApiClient";
import type { TripDto, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { useQuery } from "@tanstack/react-query";

const staleTime = 1000 * 60 * 30;
export const basePath = "trips";

export const tripConfig = {
  baseKey: basePath,
  queryKey: (id?: string) => [basePath, id],
  fetchTrip: (id: string) =>
    apiClient<TripDtoFull>(`${basePath}/${id}/analytics`),

  fetchTrips: () => apiClient<TripDto[]>(`${basePath}/`),
};

export function useTrip(tripId: string) {
  return useQuery<TripDtoFull>({
    queryKey: tripConfig.queryKey(tripId),
    queryFn: () => tripConfig.fetchTrip(tripId),
    enabled: !!tripId,
    staleTime: staleTime,
  });
}

export function useTrips() {
  return useQuery<TripDto[]>({
    queryKey: [basePath],
    queryFn: tripConfig.fetchTrips,
    staleTime: staleTime,
  });
}
