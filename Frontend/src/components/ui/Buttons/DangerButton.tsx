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

  const handleConfirmed = () => {
    setShowWarning(false);
    onConfirm();
  };

  return (
    <>
      <Button
        size={"xl"}
        fontWeight={"bold"}
        {...rest}
        colorPalette={"red"}
        onClick={handleConfirmation}
      >
        {children}
      </Button>

      <AlertDialog
        open={showWarning}
        onOpenChange={({ open }) => setShowWarning(open)}
        onConfirm={handleConfirmed}
        config={alertConfig}
      />
    </>
  );
}
export default DangerButton;
