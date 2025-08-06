import type { TripSummaries } from "@/types/ApiTypes/TripDtos";
import { For, GridItem } from "@chakra-ui/react";
import SubTitle from "../Titles/SubTitle";
import TripCard from "./Card/TripCard";
import sortTrips from "./sortTrips";
import { Fragment } from "react/jsx-runtime";

interface Props {
  trips: TripSummaries;
}

function RenderTripCards({ trips }: Props) {
  const groupedByYears = sortTrips(trips);

  return (
    <For each={groupedByYears}>
      {({ year, trips }) => (
        <Fragment key={year}>
          <GridItem color={"fg.muted"} colSpan={{ base: 1, lg: 4 }}>
            <SubTitle title={year.toString()} />
          </GridItem>
          <For each={trips}>
            {(trip) => <TripCard data={trip} key={trip.id}></TripCard>}
          </For>
        </Fragment>
      )}
    </For>
  );
}

export default RenderTripCards;
