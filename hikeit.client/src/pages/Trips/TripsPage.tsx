import { IconPlus } from "@/Icons/Icons";
import SkeletonGrid from "@/components/Placeholders/SkeletonGrid";
import PageTitle from "@/components/Titles/PageTitle";
import NoTrips from "@/components/Trips/NoTrips";
import RenderTripCards from "@/components/Trips/RenderTripCards";
import FetchWrapper from "@/components/Wrappers/Fetching";
import { useTrips } from "@/hooks/UseTrips/useTrips";
import { Button, SimpleGrid, Spacer, Stack } from "@chakra-ui/react";
import { Link } from "react-router";

function TripsPage() {
  const getTrips = useTrips();

  return (
    <Stack gap={10}>
      <Stack direction={"row"} placeItems={"baseline"} gap={8}>
        <PageTitle title="Your trips" />
        <Spacer />
        <Button
          fontWeight={"semibold"}
          colorPalette={"blue"}
          size={{ base: "sm", lg: "lg" }}
          asChild
        >
          <Link to={"add"}>
            {"New Trip"}
            <IconPlus />
          </Link>
        </Button>
      </Stack>

      <SimpleGrid
        alignItems={"stretch"}
        justifyItems={"stretch"}
        flex={1}
        columns={{ base: 1, lg: 4 }}
        gap={8}
      >
        <FetchWrapper
          request={getTrips}
          LoadingComponent={SkeletonGrid}
          NoDataComponent={NoTrips}
        >
          {(tripsData) => <RenderTripCards trips={tripsData} />}
        </FetchWrapper>
      </SimpleGrid>
    </Stack>
  );
}

export default TripsPage;
