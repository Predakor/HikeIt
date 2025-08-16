import api from "@/Utils/Api/apiRequest";
import type { RegionProgressFull } from "@/types/ApiTypes/region.types";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router";

export default function UseRegionProgress() {
  const { regionId } = useParams();

  const request = useQuery<RegionProgressFull>({
    queryKey: ["regionProgress", regionId],
    queryFn: () => api.get<RegionProgressFull>(`users/me/regions/${regionId}`),
    enabled: !!regionId,
  });

  return request;
}
