import apiClient from "@/Utils/Api/ApiClient";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";
import { basePath } from "./useTrips";
import type { TripDto } from "@/types/ApiTypes/TripDtos";

export function useTripRemove() {
  const queryClient = useQueryClient();
  const navigation = useNavigate();

  return useMutation({
    mutationFn: async (id: string) => {
      const tryDelete = await apiClient(basePath + "/" + id, {
        method: "DELETE",
      });

      //returns null in case of succes
      if (tryDelete === null) {
        return id;
      }
    },
    onSuccess: (deletedTripId) => {
      navigation(-1);
      queryClient.invalidateQueries({ queryKey: ["user-stats"] });
      queryClient.invalidateQueries({ queryKey: [basePath] });

      queryClient.setQueryData([basePath], (oldData: TripDto[]) => {
        if (!oldData) {
          return [];
        }
        return oldData.filter((trip) => trip.id !== deletedTripId);
      });
    },
  });
}
