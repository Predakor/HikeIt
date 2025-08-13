import { Button, type ButtonProps } from "@chakra-ui/react/button";
import { useState, type ReactNode } from "react";
import AlertDialog, { type AlertConfig } from "../Dialog/AlertDialog";

interface Props extends ButtonProps {
  children: ReactNode;
  onConfirm: () => void;
  alertConfig?: AlertConfig;
}

function DangerButton(props: Props) {
  const { children, title, alertConfig, onConfirm, ...rest } = props;
  const [showWarning, setShowWarning] = useState(false);

  const handleConfirmation = () => {
    setShowWarning(true);
  };

  return (
    <>
      <Button
        size={"xl"}
        {...rest}
        colorPalette={"red"}
        onClick={handleConfirmation}
      >
        {children}
      </Button>

      <AlertDialog
        open={showWarning}
        onOpenChange={({ open }) => setShowWarning(open)}
        onConfirm={onConfirm}
        config={alertConfig}
      />
    </>
  );
}
export default DangerButton;
