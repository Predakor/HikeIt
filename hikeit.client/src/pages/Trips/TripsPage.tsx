import apiClient from "@/Utils/Api/ApiClient";
import NoTrips from "@/components/Trips/NoTrips";
import Trips from "@/components/Trips/Trips";
import FetchWrapper from "@/components/Wrappers/Fetching";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { GridItem, Heading, Icon, Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { HiPlus } from "react-icons/hi2";
import { Link } from "react-router";

function TripsPage() {
  const request = useQuery<TripDto[]>({
    queryKey: ["trip"],
    queryFn: () => apiClient<TripDto[]>(`trips/`),
  });

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
