import apiClient from "@/Utils/Api/ApiClient";
import api from "@/Utils/Api/apiRequest";
import type { UserType } from "@/components/User/User";
import type { RegisterForm } from "@/pages/Auth/RegisterPage";
import { useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

export type AuthError = {
  code: string;
  description: string;
};

type AuthResponse<T> = T | AuthError[];

export function useAuth() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const removeUserQueries = () => {
    queryClient.invalidateQueries();
    queryClient.removeQueries({ queryKey: ["user"] });
  };

  const logout = async () => {
    try {
      await api.post("auth/logout", resolveAuthResponse);
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
    var response = await api.post<AuthResponse<null>>(
      "auth/login",
      {
        username,
        password,
      },
      resolveAuthResponse
    );

    const errors = response?.errors;
    if (errors) {
      return errors;
    }

    //prefetch
    queryClient.invalidateQueries({ queryKey: ["user"] });
    queryClient.refetchQueries({ queryKey: ["user"] });
    navigate("/trips");
  };

  const register = async (data: RegisterForm) => {
    var request = await api.post<AuthResponse<null>>(
      "auth/register",
      data,
      resolveAuthResponse
    );

    const errors = request?.errors;
    if (errors) {
      return errors;
    }

    return true;
  };

  return { login, logout, me, register };
}

const resolveAuthResponse = async <T>(
  response: Response
): Promise<AuthResponse<T>> => {
  if (!response.ok) {
    var result = await response.json();
    return { errors: result };
  }

  return null as T;
};
