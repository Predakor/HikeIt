import SubTitle from "@/components/Titles/SubTitle";
import { Dialog as ChakraDialog, type DialogRootProps } from "@chakra-ui/react";
import { CloseModal } from "./Common/CloseModal";
import { DialogTitle } from "./Common/DialogTitle";

interface Props extends DialogRootProps {
  title?: string;
}

function Dialog({ children, title, ...rest }: Props) {
  return (
    <ChakraDialog.Root size={"md"} motionPreset={"slide-in-bottom"} {...rest}>
      <ChakraDialog.Trigger />

      <ChakraDialog.Backdrop />
      <ChakraDialog.Positioner>
        <ChakraDialog.Content>
          <ChakraDialog.Header as={"header"} flexGrow={1}>
            <DialogTitle title={title} />
            <CloseModal />
          </ChakraDialog.Header>

          <ChakraDialog.Body>{children}</ChakraDialog.Body>

          <ChakraDialog.Footer />
        </ChakraDialog.Content>
      </ChakraDialog.Positioner>
    </ChakraDialog.Root>
  );
}

export default Dialog;
