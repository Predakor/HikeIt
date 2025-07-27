import { useForm } from "react-hook-form";
import useLogin, { type LoginForm } from "./useLogin";

const demoUserCredentials = {
  userName: "defaultuser",
  password: "Default123!",
};

export default function useLoginForm() {
  const login = useLogin();
  const formHook = useForm<LoginForm>();

  const onSubmit = formHook.handleSubmit((data) => login.mutate(data));
  const loginAsDemoUser = () => login.mutate(demoUserCredentials);

  return {
    login,
    formHook,
    onSubmit,
    loginAsDemoUser,
  } as const;
}
