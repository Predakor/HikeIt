import apiClient from "@/Utils/Api/ApiClient";
import NoTrips from "@/components/Trips/NoTrips";
import Trips from "@/components/Trips/Trips";
import FetchWrapper from "@/components/Wrappers/Fetching";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import {
  Box,
  Card,
  Grid,
  GridItem,
  Heading,
  Icon,
  Stack,
} from "@chakra-ui/react";
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
      <Box placeItems={"center"}>
        <Heading size={"5xl"}>Your trips</Heading>
      </Box>

      <Grid
        alignItems={"center"}
        justifyItems={"center"}
        templateColumns="repeat(4, 1fr)"
        gap={8}
      >
        <FetchWrapper request={request} NoDataComponent={NoTrips}>
          {(tripsData) => (
            <>
              <Trips trips={tripsData} />
              <Link to={"add"}>
                <Card.Root
                  transition="all .25s ease-in"
                  _hover={{
                    bg: "bg.emphasized",
                    transform: "scale(1.08)",
                    borderRadius: "5%",
                    transition: "all .15s ease-out",
                  }}
                >
                  <Card.Header as="center">
                    <Card.Title>New trip</Card.Title>
                  </Card.Header>
                  <Card.Body alignItems={"center"}>
                    <Icon fontSize={"5xl"} padding={".125em"}>
                      <HiPlus />
                    </Icon>
                  </Card.Body>
                  <Card.Footer>click here to add new trip</Card.Footer>
                </Card.Root>
              </Link>
            </>
          )}
        </FetchWrapper>
      </Grid>
    </Stack>
  );
}

export default TripsPage;
