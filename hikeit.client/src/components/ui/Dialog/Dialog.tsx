import { IconClose } from "@/Icons/Icons";
import {
  Dialog as ChakraDialog,
  Flex,
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
          <Flex align={"center"} justify={"space-between"}>
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

            <ChakraDialog.Header flexGrow={1}>
              <ChakraDialog.Title>{title && title}</ChakraDialog.Title>
            </ChakraDialog.Header>
          </Flex>

          <ChakraDialog.Body>{children}</ChakraDialog.Body>

          <ChakraDialog.Footer />
        </ChakraDialog.Content>
      </ChakraDialog.Positioner>
    </ChakraDialog.Root>
  );
}
export default Dialog;
