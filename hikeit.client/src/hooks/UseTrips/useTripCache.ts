import { useQueryClient } from "@tanstack/react-query";
import { basePath, tripConfig } from "./useTrips";

function useTripCache() {
  const queryClient = useQueryClient();

  const prefetchAnalytics = (tripId: string) =>
    queryClient.fetchQuery({
      queryKey: tripConfig.queryKey(tripId),
      queryFn: () => tripConfig.fetchTrip(tripId),
    });

  const prefetchTrips = () =>
    queryClient.fetchQuery({
      queryKey: [tripConfig.baseKey],
      queryFn: tripConfig.fetchTrips,
    });

  const prefetchAndCache = async (location: string) => {
    const segments = location.split("/");
    const tripId = segments[segments.length - 1];

    await Promise.all([prefetchAnalytics(tripId), prefetchTrips()]);
  };

  const invalidateTripCachces = async () => {
    queryClient.invalidateQueries({ queryKey: ["user-stats"] });
    queryClient.invalidateQueries({ queryKey: [basePath] });
  };

  return { prefetchAndCache, invalidateTripCachces };
}
export default useTripCache;
