import type { Trip } from "@/data/types";
import { Flex } from "@chakra-ui/react";
import MapWrapper from "../Wrappers/Mapping";
import TripCard from "./TripCard";

//TODO actually so some ui to show trip creation when no trips
interface Props {
  trips: Trip[];
}
function Trips({ trips }: Props) {
  return (
    <Flex gap={5}>
      <MapWrapper
        items={trips}
        renderItem={(trip) => <TripCard key={trip.id} trip={trip} />}
      />
    </Flex>
  );
}

export default Trips;
