import api from "@/Utils/Api/apiRequest";
import type { RegisterForm } from "@/hooks/Auth/useAuth";
import { tripConfig } from "@/hooks/UseTrips/useTrips";
import type { UserType } from "@/types/User/user.types";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

function useRegister() {
  const queryClient = useQueryClient();
  const navigation = useNavigate();

  const register = useMutation({
    mutationKey: ["register"],
    mutationFn: (data: RegisterForm) => api.post("auth/register", data),
    onSuccess: async (_, { userName, password }) => {
      queryClient.clear();
      queryClient.setQueryData([tripConfig.queryKey()], () => []);

      await queryClient.fetchQuery({
        queryKey: ["user", "login"],
        queryFn: () => api.post<string>("auth/login", { userName, password }),
      });

      await queryClient.prefetchQuery({
        queryKey: ["user"],
        queryFn: () => api.get<UserType>("auth/me"),
      });

      navigation("/trips");
    },
  });

  return register;
}
export default useRegister;
