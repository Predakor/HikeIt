import { Button, type ButtonProps } from "@chakra-ui/react/button";

type Props = ButtonProps;

export function PrimaryButton(props: Props) {
  return (
    <Button size={{ base: "md", lg: "xl" }} colorPalette={"blue"} {...props}>
      {props.children}
    </Button>
  );
}
