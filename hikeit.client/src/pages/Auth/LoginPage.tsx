import apiClient from "@/Utils/Api/ApiClient";
import { PasswordInput } from "@/components/ui/password-input";
import {
  Button,
  Field,
  Fieldset,
  Input,
  InputGroup,
  Separator,
  Stack,
  Text,
} from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { LuUser } from "react-icons/lu";

interface LoginForm {
  userName: string;
  password: string;
}

function LoginPage() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginForm>();

  const onSubmit = handleSubmit((data) =>
    console.log(
      apiClient("auth/login", { method: "POST", body: JSON.stringify(data) })
    )
  );

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
            <Field.Root invalid={!!errors.userName}>
              <InputGroup endElement={<LuUser size={"18"} />}>
                <Input
                  {...register("userName", {
                    required: true,
                    minLength: 3,
                    maxLength: 64,
                  })}
                  placeholder={"Username"}
                  size={"xl"}
                />
              </InputGroup>
              <Field.ErrorText>{errors.userName?.type}</Field.ErrorText>
            </Field.Root>

            <Field.Root invalid={!!errors.password}>
              <PasswordInput
                {...register("password", {
                  required: true,
                  minLength: 6,
                  maxLength: 64,
                  pattern:
                    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$/,
                })}
                placeholder={"Password"}
                size={"xl"}
              />
              <Field.ErrorText>{errors.password?.type}</Field.ErrorText>
            </Field.Root>

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
          <Text color={"fg.muted"}>Dont have a account? </Text>

          {/* //it should be a link */}
          <Button
            size={{ base: "md", lg: "lg" }}
            colorPalette={"green"}
            type={"submit"}
          >
            Register Now
          </Button>
        </Stack>
      </Fieldset.Root>
    </Stack>
  );
}

export default LoginPage;
