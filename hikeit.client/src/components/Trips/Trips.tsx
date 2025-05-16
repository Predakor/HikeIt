import type { Trip } from "@/data/types";
import useFetch from "@/hooks/useFetch";
import { Flex } from "@chakra-ui/react";
import TripCard from "./TripCard";
import FetchWrapper from "../Wrappers/Fetching";
import MapWrapper from "../Wrappers/Mapping";

function Trips() {
  const request = useFetch<Trip[]>("trips");

  return (
    <Flex gap={5}>
      <FetchWrapper request={request}>
        {(data) => (
          <MapWrapper
            items={data}
            renderItem={(trip) => <TripCard key={trip.id} trip={trip} />}
          />
        )}
      </FetchWrapper>
    </Flex>
  );
}

export default Trips;
