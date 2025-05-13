import Trips from "@/components/Trips/Trips";
import { Grid, GridItem, Heading } from "@chakra-ui/react";

function tripsPage() {
  return (
    <Grid gap={10}>
      <GridItem>
        <Heading size={"5xl"}>Your trips</Heading>
      </GridItem>

      <GridItem>
        <Trips></Trips>
      </GridItem>
    </Grid>
  );
}

export default tripsPage;
