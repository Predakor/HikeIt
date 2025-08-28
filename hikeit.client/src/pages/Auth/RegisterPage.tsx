import RegisterUserForm from "@/components/Forms/RegisterUserForm/RegisterUserForm";
import NavButton from "@/components/ui/Buttons/NavButton";
import { Fieldset, Stack, Text } from "@chakra-ui/react";

export default function RegisterPage() {
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

        <Stack gapY={4}>
          <RegisterUserForm />
          <Text color={"fg.muted"}>Already have an account? </Text>
          <NavButton to={"/auth/login"} label={"Login instead"} />
        </Stack>
      </Fieldset.Root>
    </Stack>
  );
}
