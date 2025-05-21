import AddTrip from "@/components/AddTrip/AddTrip";
import Trips from "@/components/Trips/Trips";
import { GridItem, Heading, Stack } from "@chakra-ui/react";

function TripsPage() {
  return (
    <>
      <Stack gap={10}>
        <GridItem placeItems={"center"}>
          <Heading size={"5xl"}>Your trips</Heading>
        </GridItem>

        <GridItem>
          <Trips></Trips>
        </GridItem>
        <GridItem>
          <AddTrip />
        </GridItem>
      </Stack>
    </>
  );
}

export default TripsPage;
