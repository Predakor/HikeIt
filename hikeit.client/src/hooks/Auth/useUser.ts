import api from "@/Utils/Api/apiRequest";
import type { UserType } from "@/types/User/user.types";
import { useQuery } from "@tanstack/react-query";

export default function useUser() {
  return useQuery<UserType>({
    queryKey: ["user"],
    queryFn: () => api.get<UserType>("auth/me"),
    staleTime: 1000 * 60 * 10,
    retry: false,
  });
}
