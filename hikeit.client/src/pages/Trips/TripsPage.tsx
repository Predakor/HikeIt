import type { TripDto } from "@/components/AddTripForm/AddTrip/types";
import NoTrips from "@/components/Trips/NoTrips";
import Trips from "@/components/Trips/Trips";
import FetchWrapper from "@/components/Wrappers/Fetching";
import useFetch from "@/hooks/useFetch";
import { GridItem, Heading, Icon, Stack } from "@chakra-ui/react";
import { HiPlus } from "react-icons/hi2";
import { Link } from "react-router";

function TripsPage() {
  const request = useFetch<TripDto[]>("trips");

  return (
    <Stack gap={10}>
      <GridItem placeItems={"center"}>
        <Heading size={"5xl"}>Your trips</Heading>
      </GridItem>

      <GridItem>
        <FetchWrapper request={request} NoDataComponent={NoTrips}>
          {(tripsData) => <Trips trips={tripsData} />}
        </FetchWrapper>

        <Link to={"add"}>
          <Icon
            bg={"blue"}
            borderRadius={"50%"}
            size={"2xl"}
            padding={".125em"}
          >
            <HiPlus />
          </Icon>
        </Link>
      </GridItem>
    </Stack>
  );
}

export default TripsPage;
