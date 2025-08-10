import RegisterUserForm from "@/components/Forms/RegisterUserForm/RegisterUserForm";
import NavButton from "@/components/Utils/NavButton/NavButton";
import { useAuth, type AuthError } from "@/hooks/Auth/useAuth";
import { Alert, Fieldset, Stack, Text } from "@chakra-ui/react";
import { useState } from "react";
import { useNavigate } from "react-router";

export default function RegisterPage() {
  const authActions = useAuth();
  const navigation = useNavigate();

  const [requestErrors, setRequestErrors] = useState<AuthError[] | null>(null);

  const onSubmit = async (data: any) => {
    var errors = await authActions.register(data);
    if (errors) {
      setRequestErrors(errors);
      return;
    }
    navigation("/trips");
  };

  return (
    <Stack flexGrow={1} alignItems={"center"} paddingTop={8}>
      <Fieldset.Root maxW={"lg"} gapY={8}>
        <Stack gapY={6}>
          <Fieldset.HelperText fontSize={"md"}>
            Every journey begins with small step
          </Fieldset.HelperText>
          <Fieldset.Legend fontWeight={"bold"} fontSize={"4xl"}>
            Start your's now
          </Fieldset.Legend>
        </Stack>

        <FormState errors={requestErrors} />

        <Stack gapY={4}>
          <Text color={"fg.muted"}>Already have an account? </Text>
          <RegisterUserForm submitHandler={onSubmit} />
          <NavButton to={"/auth/login"} label={"Login instead"} />
        </Stack>
      </Fieldset.Root>
    </Stack>
  );
}

function FormState({ errors }: { errors: AuthError[] | null }) {
  if (!errors || !errors.length) {
    return;
  }

  const [error] = errors;
  return (
    <Alert.Root colorPalette={"red"}>
      <Alert.Indicator />
      <Alert.Content>
        <Alert.Title>{error.code}</Alert.Title>
        <Alert.Description>{error.description}</Alert.Description>
      </Alert.Content>
    </Alert.Root>
  );
}
