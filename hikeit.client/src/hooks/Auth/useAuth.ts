import api from "@/Utils/Api/apiRequest";
import { useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

export type AuthError = {
  code: string;
  description: string;
};

export interface RegisterForm {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  userName: string;
}

type AuthResponse<T> = T | { errors: AuthError[] };

export function useAuth() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const removeUserQueries = () => {
    queryClient.clear();
  };

  const logout = () => {
    try {
      api.post("auth/logout", resolveAuthResponse);
      removeUserQueries();
      navigate("/auth/login");
    } catch (error) {}
  };

  const login = async (username: string, password: string) => {
    const response = await api.post<AuthResponse<null>>(
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
    const request = await api.post<AuthResponse<null>>(
      "auth/register",
      data,
      resolveAuthResponse
    );

    const errors = request?.errors;
    if (errors) {
      return errors;
    }

    return null;
  };

  return { login, logout, register };
}

const resolveAuthResponse = async <T>(
  response: Response
): Promise<AuthResponse<T>> => {
  if (!response.ok) {
    const result = await response.json();
    return { errors: result };
  }

  return null as T;
};
