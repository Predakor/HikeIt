import apiClient from "@/Utils/Api/ApiClient";
import AddTripCard from "@/components/Trips/Card/AddTripCard";
import NoTrips from "@/components/Trips/NoTrips";
import RenderTripCards from "@/components/Trips/RenderTripCards";
import FetchWrapper from "@/components/Wrappers/Fetching";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { Box, Grid, Heading, Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { Link } from "react-router";

function TripsPage() {
  const request = useQuery<TripDto[]>({
    queryKey: ["trip"],
    queryFn: () => apiClient<TripDto[]>(`trips/`),
    staleTime: 1000 * 60 * 30,
  });

  return (
    <Stack gap={10}>
      <Box placeItems={"center"}>
        <Heading size={"5xl"}>Your trips</Heading>
      </Box>

      <Grid
        alignItems={"stretch"}
        justifyItems={"stretch"}
        templateColumns={{ base: "", md: "repeat(4, 1fr)" }}
        gap={8}
      >
        <FetchWrapper request={request} NoDataComponent={NoTrips}>
          {(tripsData) => (
            <>
              <RenderTripCards trips={tripsData} />

              <Link to={"add"}>
                <AddTripCard />
              </Link>
            </>
          )}
        </FetchWrapper>
      </Grid>
    </Stack>
  );
}

export default TripsPage;
