import { resolveCreated } from "@/Utils/Api/Resolvers/resolveCreated";
import api from "@/Utils/Api/apiRequest";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { basePath, tripConfig } from "./useTrips";

export function useTripCreate() {
  const queryClient = useQueryClient();

  const prefetchAndCache = async (location: string) => {
    const segments = location.split("/");
    const tripId = segments[segments.length - 1];

    const prefetchAnalytics = queryClient.fetchQuery({
      queryKey: tripConfig.queryKey(tripId),
      queryFn: () => tripConfig.fetchTrip(tripId),
    });

    const prefetchTrips = queryClient.fetchQuery({
      queryKey: [tripConfig.baseKey],
      queryFn: tripConfig.fetchTrips,
    });

    await Promise.all([prefetchAnalytics, prefetchTrips]);
  };

  return useMutation({
    mutationFn: (data: object) =>
      api.post(`${basePath}/form`, data, resolveCreated),
    onSuccess: async ({ location }) => {
      queryClient.invalidateQueries({ queryKey: ["user-stats"] });
      queryClient.invalidateQueries({ queryKey: [basePath] });

      if (location) {
        prefetchAndCache(location);
      }
    },
  });
}
