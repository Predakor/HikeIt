import usePagePreload from "@/hooks/Utils/usePagePreload";
import type { TripSummaries } from "@/types/ApiTypes/TripDtos";
import { For, GridItem } from "@chakra-ui/react";
import { Fragment } from "react/jsx-runtime";
import SubTitle from "../Titles/SubTitle";
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
