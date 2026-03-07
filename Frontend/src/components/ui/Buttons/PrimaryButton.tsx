import { Button, type ButtonProps } from "@chakra-ui/react/button";

interface Props extends ButtonProps {}

export function PrimaryButton(props: Props) {
  return (
    <Button size={{ base: "md", lg: "xl" }} colorPalette={"blue"} {...props}>
      {props.children}
    </Button>
  );
}
