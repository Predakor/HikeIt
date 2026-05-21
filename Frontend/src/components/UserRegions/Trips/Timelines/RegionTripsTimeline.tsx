import { IconJourney } from "@/Icons/Icons";
import { formatter } from "@/components/User/Stats/Utils/formatter";
import type { RegionTrip } from "@/types/Api/TripDtos";
import { Card, For, Stack, Stat, Timeline } from "@chakra-ui/react";
import { Link } from "react-router";

export default function RegionTripsTimeline({ trips }: { trips: RegionTrip[] }) {
  const sortedTrips = trips.sort(
    (a, b) => new Date(a.tripDay).getTime() - new Date(b.tripDay).getTime(),
  );

  return (
    <Timeline.Root variant={"subtle"} maxWidth={"2xl"} size={"xl"}>
      <For each={sortedTrips}>
        {(trip) => (
          <Timeline.Item>
            <Timeline.Content w={"auto"}>
              <Timeline.Title whiteSpace={"nowrap"} textStyle="xl">
                {trip.tripDay}
              </Timeline.Title>
            </Timeline.Content>
            <Timeline.Connector>
              <Timeline.Separator />
              <Timeline.Indicator>
                <IconJourney size={"16"} color="white" />
              </Timeline.Indicator>
            </Timeline.Connector>
            <Timeline.Content gap={4}>
              <Link to={`/trips/${trip.id}`}>
                <Card.Root>
                  <Card.Header>
                    <Timeline.Title textStyle={"xl"}>{trip.name}</Timeline.Title>
                  </Card.Header>
                  <Card.Body>
                    <Stack direction={"row"}>
                      <Stat.Root>
                        <Stat.Label>Distance</Stat.Label>
                        <Stat.ValueText>{formatter.toKm(trip.distance)} km</Stat.ValueText>
                      </Stat.Root>

                      {trip.duration && (
                        <Stat.Root>
                          <Stat.Label>Duration</Stat.Label>
                          <Stat.ValueText>{formatter.toDuration(trip.duration)}</Stat.ValueText>
                        </Stat.Root>
                      )}
                    </Stack>
                  </Card.Body>
                </Card.Root>
              </Link>
            </Timeline.Content>
          </Timeline.Item>
        )}
      </For>
    </Timeline.Root>
  );
}
