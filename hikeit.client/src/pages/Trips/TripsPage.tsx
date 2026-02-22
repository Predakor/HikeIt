import { IconPlus } from "@/Icons/Icons";
import SkeletonGrid from "@/components/Placeholders/SkeletonGrid";
import NoTrips from "@/components/Trips/NoTrips";
import RenderTripCards from "@/components/Trips/RenderTripCards";
import FetchWrapper from "@/components/Utils/Fetching";
import PageTitle from "@/components/ui/Titles/PageTitle";
import { useTrips } from "@/hooks/UseTrips/useTrips";
import usePagePreload from "@/hooks/Utils/usePagePreload";
import { Button, Spacer, Stack } from "@chakra-ui/react";
import { Link } from "react-router";

function TripsPage() {
  const getTrips = useTrips();
  usePagePreload("/trips/add");
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

      <Stack alignItems={"stretch"} justifyItems={"stretch"} flex={1} gap={8}>
        <FetchWrapper request={getTrips} LoadingComponent={SkeletonGrid} NoDataComponent={NoTrips}>
          {(tripsData) => <RenderTripCards trips={tripsData} />}
        </FetchWrapper>
      </Stack>
    </Stack>
  );
}

export default TripsPage;
