import NoTrips from "@/components/Trips/NoTrips";
import Trips from "@/components/Trips/Trips";
import FetchWrapper from "@/components/Wrappers/Fetching";
import type { Trip } from "@/data/types";
import useFetch from "@/hooks/useFetch";
import { GridItem, Heading, Stack } from "@chakra-ui/react";

function TripsPage() {
  const request = useFetch<Trip[]>("trips");

  return (
    <Stack gap={10}>
      <GridItem placeItems={"center"}>
        <Heading size={"5xl"}>Your trips</Heading>
      </GridItem>

      <GridItem>
        <FetchWrapper request={request} NoDataComponent={NoTrips}>
          {(tripsData) => <Trips trips={tripsData} />}
        </FetchWrapper>
      </GridItem>
    </Stack>
  );
}

export default TripsPage;
