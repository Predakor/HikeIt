import api from "@/Utils/Api/apiRequest";
import type { UserRegionData } from "@/types/Api/region.types";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";

export default function useUserRegion() {
  const { regionId } = useParams();

  const request = useQuery<UserRegionData>({
    queryKey: ["regionProgress", regionId],
    queryFn: () => api.get<UserRegionData>(`users/me/regions/${regionId}`),
    enabled: !!regionId,
  });

  return request;
}
