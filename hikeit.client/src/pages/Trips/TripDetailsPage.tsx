import apiClient from "@/Utils/Api/ApiClient";
import TripDetails from "@/components/Trip/TripDetails/TripDetails";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";

const staleTime = 1000 * 60 * 30; //1000ms * 60* 30 //30 minuts;

export default function TripDetailsPage() {
  const { tripId } = useParams();

  const request = useQuery<TripDtoFull>({
    queryKey: ["trip", tripId],
    queryFn: () => apiClient<TripDtoFull>(`trips/${tripId}/analytics`),
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
