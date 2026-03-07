import api from "@/Utils/Api/apiRequest";
import { useQueryClient, useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router";

export interface LoginForm {
  userName: string;
  password: string;
}

type LoginError = {
  message: string;
  description: string;
};

export default function useLoginForm() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const login = useMutation<null, LoginError, LoginForm>({
    mutationFn: async (form: LoginForm) => await api.post("auth/login", form),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["user"] });
      queryClient.refetchQueries({ queryKey: ["user"] });
      navigate("/trips");
    },
  });

  const loginAsDemoUser = () => login.mutate(demoUserCredentials);

  return {
    login,
    loginAsDemoUser,
  } as const;
}

const demoUserCredentials = {
  userName: "defaultuser",
  password: "Default123!",
};
