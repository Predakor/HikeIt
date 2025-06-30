import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { Button, Fieldset, Separator } from "@chakra-ui/react";
import useRegister from "./useRegister";

interface Props {
  submitHandler: Function;
}

export default function RegisterUserForm({ submitHandler }: Props) {
  const [formHook, config] = useRegister();

  return (
    <form onSubmit={formHook.handleSubmit((data) => submitHandler(data))}>
      <Fieldset.Content>
        <RenderInputs
          config={config}
          formHook={formHook}
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
  );
}
