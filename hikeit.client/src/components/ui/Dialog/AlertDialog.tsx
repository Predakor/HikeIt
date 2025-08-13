import { ToTitleCase } from "@/Utils/ObjectToArray";
import {
  Button,
  Dialog as ChakraDialog,
  Portal,
  type DialogRootProps,
} from "@chakra-ui/react";
import type { ReactNode } from "react";
import { CloseModal } from "./Common/CloseModal";
import { DialogTitle } from "./Common/DialogTitle";

export type AlertConfig = {
  title?: string;
  confirmButtonText?: string;
  warningDescription?: ReactNode;
};

export interface AlertDialogProps
  extends Pick<DialogRootProps, "open" | "onOpenChange"> {
  onConfirm: () => void;
  config?: AlertConfig;
}

export default function AlertDialog(props: AlertDialogProps) {
  const { config, onConfirm, ...rest } = props;

  return (
    <ChakraDialog.Root
      size={"lg"}
      {...rest}
      role="alertdialog"
      motionPreset={"scale"}
    >
      <ChakraDialog.Trigger />

      <Portal>
        <ChakraDialog.Backdrop />

        <ChakraDialog.Positioner>
          <ChakraDialog.Content>
            <ChakraDialog.Header as={"header"} flexGrow={1}>
              <DialogTitle title={config?.title ?? "Confirm Action"} />
              <CloseModal />
            </ChakraDialog.Header>

            <ChakraDialog.Body fontSize={{ base: "lg" }}>
              {config?.warningDescription ??
                "This action can't be reverted are you sure"}
            </ChakraDialog.Body>

            <ChakraDialog.Footer>
              <ChakraDialog.ActionTrigger asChild>
                <Button size={{ base: "lg", lg: "xl" }} variant="outline">
                  Cancel
                </Button>
              </ChakraDialog.ActionTrigger>
              <Button
                onClick={onConfirm}
                size={{ base: "lg", lg: "xl" }}
                colorPalette="red"
              >
                {ToTitleCase(config?.confirmButtonText ?? "delete")}
              </Button>
            </ChakraDialog.Footer>
          </ChakraDialog.Content>
        </ChakraDialog.Positioner>
      </Portal>
    </ChakraDialog.Root>
  );
}
