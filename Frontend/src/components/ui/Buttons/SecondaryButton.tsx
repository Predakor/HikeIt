import { Button, type ButtonProps } from "@chakra-ui/react/button";

interface Props extends ButtonProps {}

export function SecondaryButton(props: Props) {
  return (
    <Button size={{ base: "md", lg: "xl" }} colorPalette={"green"} {...props}>
      {props.children}
    </Button>
  );
}
