import apiClient from "@/Utils/Api/ApiClient";
import type { TripDtoFull } from "@/components/AddTripForm/AddTrip/types";
import TripDetails from "@/components/Trip/TripDetails";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";

const staleTime = 1000 * 60 * 30; //1000ms * 60* 30 //30 minuts;

function TripDetailsPage() {
  const { tripId } = useParams();

  const request = useQuery<TripDtoFull>({
    queryKey: ["trip", tripId],
    queryFn: () => apiClient<TripDtoFull>(`trips/${tripId}`),
    enabled: !!tripId,
    staleTime: staleTime,
  });

  return (
    <FetchWrapper
      request={request}
      LoadingComponent={() => "wait i'm loading"}
      Component={TripDetails}
    />
  );
}
export default TripDetailsPage;
