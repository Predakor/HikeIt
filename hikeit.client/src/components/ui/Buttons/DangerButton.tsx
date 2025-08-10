import { Button, type ButtonProps } from "@chakra-ui/react/button";
import type { ReactElement } from "react";

interface Props extends ButtonProps {
  children: ReactElement | string | number;
}

function DangerButton(props: Props) {
  const { children, ...rest } = props;
  const handleConfirmation = () => {
    window.alert("you're about to do something you cant revert are you sure?");
  };
  return (
    <Button
      size={"xl"}
      {...rest}
      colorPalette={"red"}
      onClick={handleConfirmation}
    >
      {children}
    </Button>
  );
}
export default DangerButton;
