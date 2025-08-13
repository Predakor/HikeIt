import { IconClose } from "@/Icons/Icons";
import { Dialog as ChakraDialog, IconButton } from "@chakra-ui/react";

export function CloseModal() {
  return (
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
  );
}
