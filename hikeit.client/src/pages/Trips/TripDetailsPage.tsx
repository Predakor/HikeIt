import apiClient from "@/Utils/Api/ApiClient";
import type { TripDto } from "@/components/AddTripForm/AddTrip/types";
import { Heading, Stack, Stat } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";

const staleTime = 1000 * 60 * 30;

function TripDetailsPage() {
  const { tripId } = useParams();

  const { data: trip, isLoading } = useQuery<TripDto>({
    queryKey: ["trip", tripId],
    queryFn: () => apiClient<TripDto>(`trips/${tripId}`),
    enabled: !!tripId,
    staleTime: staleTime,
  });

  if (isLoading) {
    return <div>loading</div>;
  }
  if (!trip) {
    return <div>No data</div>;
  }

  return (
    <Stack gap={"2em"}>
      <Heading>Trip summary for {trip.id}</Heading>
      <Stack gap={"1em"}>
        <Stat.Root>
          <Stat.Label>Total distance</Stat.Label>
          <Stat.ValueText>{trip.distance}</Stat.ValueText>
        </Stat.Root>
        <Stat.Root>
          <Stat.Label>Total climb</Stat.Label>
          <Stat.ValueText>{trip.height}</Stat.ValueText>
        </Stat.Root>
        <Stat.Root>
          <Stat.Label>Total time</Stat.Label>
          <Stat.ValueText>{trip.duration}</Stat.ValueText>
        </Stat.Root>
      </Stack>
    </Stack>
  );
}
export default TripDetailsPage;
