import type { TripDto } from "@/types/ApiTypes/TripDtos";
import MapWrapper from "../Wrappers/Mapping";
import TripCard from "./Card/TripCard";

interface Props {
  trips: TripDto[];
}

function RenderTripCards({ trips }: Props) {
  return (
    <MapWrapper
      items={trips}
      renderItem={(trip) => <TripCard data={trip} key={trip.id} />}
    />
  );
}

export default RenderTripCards;
