import apiClient from "@/Utils/Api/ApiClient";
import type { UserType } from "@/components/User/User";
import { useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

export function useAuth() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const removeUserQueries = () => {
    queryClient.invalidateQueries();
    queryClient.removeQueries({ queryKey: ["user"] });
  };

  const logout = async () => {
    try {
      await apiClient("auth/logout", {
        method: "POST",
      });
      removeUserQueries();
      navigate("/auth/login");
    } catch (error) {}
  };

  const me = async (): Promise<UserType> => {
    try {
      var user = await apiClient<UserType>("auth/me");
      return user;
    } catch (error) {
      throw error;
    }
  };

  const login = async (username: string, password: string) => {
    try {
      await apiClient("auth/login", {
        method: "POST",
        body: JSON.stringify({
          username,
          password,
        }),
      });

      //prefetch
      queryClient.invalidateQueries({ queryKey: ["user"] });
      queryClient.refetchQueries({ queryKey: ["user"] });
      navigate("/trips");
    } catch (error) {}
  };

  return { login, logout, me };
}
