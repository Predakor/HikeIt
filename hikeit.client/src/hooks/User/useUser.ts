import { useQuery } from "@tanstack/react-query";
import { useAuth } from "../Auth/useAuth";
import type { UserType } from "@/types/User/user.types";

export function useAdminUser() {
  const authActions = useAuth();

  const user = useQuery<UserType>({
    queryKey: ["user"],
    queryFn: authActions.me,
    staleTime: 1000 * 60 * 10,
    retry: false,
  });

  return user;
}
