import useRegionMatcher from "@/hooks/useRegionMatcher";
import { Flex, Grid } from "@chakra-ui/react";
import MapWrapper from "../Wrappers/Mapping";
import TripCard from "./TripCard";
import type { TripDto } from "@/types/ApiTypes/TripDtos";

//TODO actually so some ui to show trip creation when no trips
interface Props {
  trips: TripDto[];
}

function Trips({ trips }: Props) {
  //TODO actually implement caching fetch with global store store or context or whatever
  const { matchRegion } = useRegionMatcher();

  return (
    <MapWrapper
      items={trips}
      renderItem={(trip) => <TripCard data={trip} key={trip.id} />}
    />
  );
}

export default Trips;
