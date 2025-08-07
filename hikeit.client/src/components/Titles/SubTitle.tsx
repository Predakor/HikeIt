import { Heading } from "@chakra-ui/react";

interface Props {
  title: string;
}

export default function SubTitle({ title }: Props) {
  return (
    <Heading as={"h3"} color={"inherit"} size={{ base: "lg", lg: "xl" }}>
      {title}
    </Heading>
  );
}
