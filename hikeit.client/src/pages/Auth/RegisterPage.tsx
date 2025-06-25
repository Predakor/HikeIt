import NavButton from "@/components/Utils/NavButton/NavButton";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { useAuth } from "@/hooks/useAuth";
import {
  Button,
  Fieldset,
  Separator,
  SimpleGrid,
  Stack,
  Text,
} from "@chakra-ui/react";
import { useForm } from "react-hook-form";

export interface RegisterForm {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  userName: string;
}

const registerFormConfgig: InputsConfig = [
  {
    key: "userName",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
  {
    key: "email",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
  {
    key: "firstName",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
  {
    key: "lastName",
    label: "",
    type: "text",
    min: 3,
    max: 64,
    required: true,
  },
  {
    key: "password",
    label: "",
    type: "password",
    min: 6,
    max: 64,
    pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$/,
    required: true,
  },
];

function LoginPage() {
  const authActions = useAuth();

  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterForm>();

  const onSubmit = handleSubmit(async (data) => {
    var x = await authActions.register(data);
    console.log(x);
  });

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

        <form onSubmit={(d) => onSubmit(d)}>
          <Fieldset.Content>
            <RenderInputs
              config={registerFormConfgig}
              register={register}
              control={control}
              displayOptions={{ label: "inline", size: "xl" }}
            />

            <Button
              size={{ base: "xl", lg: "2xl" }}
              colorPalette={"blue"}
              type={"submit"}
            >
              Log in
            </Button>
            <Separator size={"sm"} flex="1" />
          </Fieldset.Content>
        </form>

        <Stack gapY={4}>
          <Text color={"fg.muted"}>Already have an account? </Text>

          <NavButton to={"/auth/login"} label={"Login instead"} />
        </Stack>
      </Fieldset.Root>
    </Stack>
  );
}

export default LoginPage;
