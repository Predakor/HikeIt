import { ToTitleCase } from "@/Utils/ObjectToArray";
import { Heading, type HeadingProps } from "@chakra-ui/react";

interface Props extends Omit<HeadingProps, "as"> {
  title: string;
}

export default function SubTitle({ title, ...rest }: Props) {
  return (
    <Heading
      as={"h3"}
      color={"inherit"}
      fontWeight={"semibold"}
      size={{ base: "lg", lg: "2xl" }}
      {...rest}
    >
      {ToTitleCase(title)}
    </Heading>
  );
}
