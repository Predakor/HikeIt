import NavButton from "@/components/Utils/NavButton/NavButton";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import type { LoginForm } from "@/hooks/Auth/useLogin";
import useLogin from "@/hooks/Auth/useLogin";
import {
  Alert,
  Button,
  Fieldset,
  Separator,
  Stack,
  Text,
} from "@chakra-ui/react";
import type { UseMutationResult } from "@tanstack/react-query";
import { useForm } from "react-hook-form";

const LoginFormCongif: InputsConfig = [
  {
    key: "userName",
    type: "text",
    label: "",
    min: 3,
    max: 64,
    pattern: /^[a-zA-Z][a-zA-Z0-9_]{2,15}$/,
    required: true,
  },
  {
    key: "password",
    type: "password",
    label: "",
    min: 6,
    max: 64,
    pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$/,
    required: true,
  },
];

function LoginPage() {
  const login = useLogin();
  const formHook = useForm<LoginForm>();

  const onSubmit = formHook.handleSubmit((data) => login.mutate(data));
  const loginAsDemoUser = () =>
    login.mutate({
      userName: "defaultuser",
      password: "Default123!",
    });

  return (
    <Stack flexGrow={1} alignItems={"center"} paddingTop={8}>
      <Fieldset.Root maxW={"lg"} gapY={8}>
        <Stack gapY={6}>
          <Fieldset.HelperText fontSize={"md"}>
            Ready for a new journey
          </Fieldset.HelperText>
          <Fieldset.Legend fontWeight={"bold"} fontSize={"4xl"}>
            Welcome back
          </Fieldset.Legend>
        </Stack>

        <form onSubmit={onSubmit}>
          <Fieldset.Content gapY={6}>
            <FormState result={login as any} />
            <RenderInputs
              config={LoginFormCongif}
              formHook={formHook}
              displayOptions={{
                size: "xl",
                label: "inline",
              }}
            />

            <Button
              size={{ base: "lg", lg: "xl" }}
              colorPalette={"blue"}
              type={"submit"}
            >
              Log in
            </Button>
            <Button
              size={{ base: "lg", lg: "xl" }}
              colorPalette={"green"}
              type={"button"}
              onClick={loginAsDemoUser}
            >
              Use Demo Account
            </Button>
            <Separator size={"sm"} flex="1" />
          </Fieldset.Content>
        </form>

        <Stack gapY={4}>
          <Text color={"fg.muted"}>Dont have a account? </Text>
          <NavButton to={"/auth/register"} label={"Register instead"} />
        </Stack>
      </Fieldset.Root>
    </Stack>
  );
}

function FormState({ result }: { result: UseMutationResult }) {
  if (!result.isError) {
    return;
  }

  return (
    <Alert.Root status={result.status}>
      <Stack>
        <Alert.Title>Error occured</Alert.Title>
        <Alert.Content>
          <Alert.Description>{result.error.message}</Alert.Description>
        </Alert.Content>
      </Stack>
    </Alert.Root>
  );
}

export default LoginPage;
