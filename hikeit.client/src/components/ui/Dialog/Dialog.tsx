import { IconClose } from "@/Icons/Icons";
import SubTitle from "@/components/Titles/SubTitle";
import {
  Dialog as ChakraDialog,
  IconButton,
  type DialogRootProps,
} from "@chakra-ui/react";

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
            <ChakraDialog.Title asChild>
              {title && <SubTitle title={title} />}
            </ChakraDialog.Title>
            <ChakraDialog.CloseTrigger asChild>
              <IconButton
                size={"2xl"}
                padding={0}
                variant={"ghost"}
                aria-label="Close modal"
              >
                <IconClose />
              </IconButton>
            </ChakraDialog.CloseTrigger>
          </ChakraDialog.Header>

          <ChakraDialog.Body>{children}</ChakraDialog.Body>

          <ChakraDialog.Footer />
        </ChakraDialog.Content>
      </ChakraDialog.Positioner>
    </ChakraDialog.Root>
  );
}
export default Dialog;
