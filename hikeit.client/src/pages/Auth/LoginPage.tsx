import schemas from "@/Utils/Schemas";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { PrimaryButton, SecondaryButton } from "@/components/ui/Buttons";
import NavButton from "@/components/ui/Buttons/NavButton";
import { MutationResult } from "@/components/ui/Results/MutationResult";
import useLogin, { type LoginForm } from "@/hooks/Auth/useLogin";
import { Fieldset, Separator, Stack, Text } from "@chakra-ui/react";
import { useForm } from "react-hook-form";

const loginFormConfig: InputsConfig = [
  schemas.login,
  schemas.password,
] as InputsConfig;

function LoginPage() {
  const formHook = useForm<LoginForm>();
  const { login, loginAsDemoUser } = useLogin();

  const loadingProps = {
    loading: login.isPending,
    loadingText: "logging",
  };

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

        <MutationResult
          requestState={login}
          errorMessage={{
            title: "Failed to login",
            description: login.error?.message!,
          }}
        />

        <form onSubmit={formHook.handleSubmit(login.mutate as any)}>
          <Fieldset.Content gapY={6}>
            <RenderInputs
              config={loginFormConfig}
              formHook={formHook}
              displayOptions={{
                size: "xl",
                label: "inline",
              }}
            />

            <PrimaryButton {...loadingProps} type={"submit"}>
              Log in
            </PrimaryButton>
            <SecondaryButton {...loadingProps} onClick={loginAsDemoUser}>
              Use Demo Account
            </SecondaryButton>
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

export default LoginPage;
