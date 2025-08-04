import TripDetails from "@/components/Trip/TripDetails/TripDetails";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import { useTrip } from "@/hooks/UseTrips/useTrips";
import { Spinner } from "@chakra-ui/react";
import { useParams } from "react-router";

export default function TripDetailsPage() {
  const { tripId } = useParams();

  const getTripDetails = useTrip(tripId!);

  return (
    <FetchWrapper
      request={getTripDetails}
      LoadingComponent={Spinner}
      Component={TripDetails}
    />
  );
}
