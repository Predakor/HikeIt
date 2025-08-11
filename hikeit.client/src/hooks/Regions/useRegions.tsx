import api from "@/Utils/Api/apiRequest";
import type { Region } from "@/types/ApiTypes/types";
import { useQuery } from "@tanstack/react-query";

export default function useRegions() {
  return useQuery({
    queryKey: ["regions"],
    queryFn: () => api.get<Region[]>("regions"),
    staleTime: 1000 * 60 * 60,
  });
}
