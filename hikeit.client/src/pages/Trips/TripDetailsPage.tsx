import TripDetailsHeader from "@/components/Trip/Details/TripDetailsHeader";
import { TripDetailsMenu } from "@/components/Trip/Details/TripDetailsMenu/TripDetailsTabs";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { useTrip } from "@/hooks/UseTrips/useTrip";
import { Spinner, Stack, VStack } from "@chakra-ui/react";

export default function TripDetailsPage() {
  const getTripDetails = useTrip();
  return (
    <FetchWrapper request={getTripDetails} LoadingComponent={Spinner}>
      {(data) => (
        <VStack alignItems={"start"} gap={8}>
          <TripDetailsHeader trip={data} />
          <Stack w={"full"} justifyItems={"center"} gap={8}>
            <TripDetailsMenu data={data.analytics} />
          </Stack>
        </VStack>
      )}
    </FetchWrapper>
  );
}
