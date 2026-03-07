import api from "@/Utils/Api/apiRequest";
import { cacheTimes } from "@/Utils/Api/staleTimes";
import type { UserProfile } from "@/types/User/user.types";
import { useQuery } from "@tanstack/react-query";

export const config = {
  key: ["user", "profile"],
};

export default function useUserProfile() {
  return useQuery({
    queryKey: ["user", "profile"],
    queryFn: () => api.get<UserProfile>("users/me/profile"),
    staleTime: cacheTimes.day,
  });
}
