import apiClient from "@/Utils/Api/ApiClient";
import type {
  TripDto,
  TripDtoFull,
} from "@/components/AddTripForm/AddTrip/types";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import { Heading, Show, Stack, Stat } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";

const staleTime = 1000 * 60 * 30;

function TripDetailsPage() {
  const { tripId } = useParams();

  const request = useQuery<TripDtoFull>({
    queryKey: ["trip", tripId],
    queryFn: () => apiClient<TripDtoFull>(`trips/${tripId}`),
  enabled: !!tripId,
    staleTime: staleTime,
  });

  return (
    <FetchWrapper request={request}>
      {({ base: trip, id, region }) => {
        console.log(trip);
        return (
          <Stack gap={"2em"}>
            <Heading>Trip summary for {id}</Heading>
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
              <Show when={region}>
                {(r) => (
                  <Stat.Root>
                    <Stat.Label>Region</Stat.Label>
                    <Stat.ValueText>{r.name}</Stat.ValueText>
                  </Stat.Root>
                )}
              </Show>
            </Stack>
          </Stack>
        );
      }}
    </FetchWrapper>
  );
}
export default TripDetailsPage;
