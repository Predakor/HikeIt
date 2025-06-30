import api from "@/Utils/Api/apiRequest";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

export interface LoginForm {
  userName: string;
  password: string;
}

type LoginError = {
  message: string;
  description: string;
};

export default function useLogin() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const resolver = async (result: Response) => {
    const resp = await result.json();

    if (!result.ok) {
      throw resp;
    }
    return resp;
  };

  const mutation = useMutation<null, LoginError, LoginForm>({
    mutationFn: async (form: LoginForm) =>
      await api.post("auth/login", form, resolver),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["user"] });
      queryClient.refetchQueries({ queryKey: ["user"] });
      navigate("/trips");
    },
  });

  return mutation;
}
