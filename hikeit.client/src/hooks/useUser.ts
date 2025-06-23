import { useQuery } from "@tanstack/react-query";
import { useAuth } from "./useAuth";
import type { UserType } from "@/components/User/User";

export default function useUser() {
  const authActions = useAuth();

  const { data: user } = useQuery<UserType>({
    queryKey: ["user"],
    queryFn: authActions.me,
    staleTime: 1000 * 60 * 10,
    retry: false,
  });

  return [user, authActions] as const;
}
