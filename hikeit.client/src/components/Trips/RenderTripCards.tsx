import usePagePreload from "@/hooks/Utils/usePagePreload";
import type { TripSummaries } from "@/types/Api/TripDtos";
import { For, GridItem, Stack } from "@chakra-ui/react";
import { Fragment } from "react/jsx-runtime";
import { formatter } from "../User/Stats/Utils/formatter";
import SubTitle from "../ui/Titles/SubTitle";
import TripCard from "./Card/TripCard";
import sortTrips from "./sortTrips";

interface Props {
  trips: TripSummaries;
}

function RenderTripCards({ trips }: Props) {
  usePagePreload("trips/:tripId");
  const groupedByYears = sortTrips(trips);

  return (
    <For each={groupedByYears}>
      {({ year, trips, totalDistance, totalDuration }) => (
        <Fragment key={year}>
          <GridItem color={"fg.muted"} colSpan={{ base: 1, lg: 4 }}>
            <Stack direction={"row"} align={"center"} gap={"4"}>
              <SubTitle title={year.toString()} />
              <div>{formatter.toKm(totalDistance)} KM</div>
              <div>{totalDuration.toMinutes()} MINUTES</div>
            </Stack>
          </GridItem>
          <For each={trips}>{(trip) => <TripCard data={trip} key={trip.id} />}</For>
        </Fragment>
      )}
    </For>
  );
}

export default RenderTripCards;
