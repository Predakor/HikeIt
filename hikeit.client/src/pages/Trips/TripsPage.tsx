import AddTripCard from "@/components/Trips/Card/AddTripCard";
import NoTrips from "@/components/Trips/NoTrips";
import RenderTripCards from "@/components/Trips/RenderTripCards";
import FetchWrapper from "@/components/Wrappers/Fetching";
import { useTrips } from "@/hooks/useTrips";
import { Box, Grid, Heading, Stack } from "@chakra-ui/react";
import { Link } from "react-router";

function TripsPage() {
  const request = useTrips();

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
          {(tripsData) => {
            return (
              <>
                <RenderTripCards trips={tripsData} />

                <Link to={"add"}>
                  <AddTripCard />
                </Link>
              </>
            );
          }}
        </FetchWrapper>
      </Grid>
    </Stack>
  );
}

export default TripsPage;
