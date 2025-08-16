import api from "@/Utils/Api/apiRequest";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";
import { basePath } from "./useTrips";

export function useTripRemove() {
  const queryClient = useQueryClient();
  const navigation = useNavigate();

  return useMutation({
    mutationFn: async (id: string) => {
      const response = await api.deleteR(`trips/${id}`);

      //returns null in case of succes
      if (response === null) {
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
