import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { PrimaryButton } from "@/components/ui/Buttons";
import { MutationResult } from "@/components/ui/Results/MutationResult";
import { type RegisterForm } from "@/hooks/Auth/useAuth";
import { Fieldset, Separator } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { registerFormConfgig } from "./registerFormConfig";
import useRegister from "./useRegister";

export default function RegisterUserForm() {
  const form = useForm<RegisterForm>();
  const register = useRegister();

  return (
    <>
      <MutationResult
        requestState={register}
        errorMessage={{
          title: register.error?.name as string,
          description: register.error?.message as string,
        }}
      />

      <form onSubmit={form.handleSubmit((data) => register.mutateAsync(data))}>
        <Fieldset.Content>
          <RenderInputs
            config={registerFormConfgig}
            formHook={form}
            displayOptions={{ label: "inline", size: "xl" }}
          />

          <PrimaryButton
            size={{ base: "xl", lg: "2xl" }}
            disabled={register.isPending}
            loading={register.isPending}
            loadingText={"Registering"}
            colorPalette={"blue"}
            type={"submit"}
          >
            Register
          </PrimaryButton>
          <Separator size={"sm"} flex="1" />
        </Fieldset.Content>
      </form>
    </>
  );
}
