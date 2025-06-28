import apiClient from "@/Utils/Api/ApiClient";
import api from "@/Utils/Api/apiRequest";
import type { TripDto, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

const staleTime = 1000 * 60 * 30;
const basePath = "trips";

export function useTrip(tripId: string) {
  return useQuery<TripDtoFull>({
    queryKey: [basePath, tripId],
    queryFn: () => apiClient<TripDtoFull>(`${basePath}/${tripId}/analytics`),
    enabled: !!tripId,
    staleTime: staleTime,
  });
}

export function useTrips() {
  return useQuery<TripDto[]>({
    queryKey: [basePath],
    queryFn: () => apiClient<TripDto[]>(`${basePath}/`),
    staleTime: staleTime,
  });
}

export function useTripRemove() {
  const queryClient = useQueryClient();
  const navigation = useNavigate();

  return useMutation({
    mutationFn: (id: string) => {
      return apiClient(basePath + "/" + id, { method: "DELETE" });
    },
    onSuccess: () => {
      navigation(-1);
      return queryClient.invalidateQueries({ queryKey: [basePath] });
    },
  });
}

export function useTripCreate() {
  const queryClient = useQueryClient();

  const requestResolver = async (response: Response) => {
    if (!response.ok) {
      throw new Error();
    }

    const res = {
      location: "",
    };

    //created
    if (response.status === 201) {
      for (const [key, value] of response.headers.entries()) {
        console.log(`Header: ${key} => ${value}`);
      }
      const location = response.headers.get("Location");
      if (location) {
        res.location = location;
      }
    }
    return res;
  };

  return useMutation({
    mutationFn: (data: object) =>
      api.post(`${basePath}/form`, data, requestResolver),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: [basePath] }),
  });
}
