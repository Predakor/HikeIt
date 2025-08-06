import { Heading } from "@chakra-ui/react";

interface Props {
  title: string;
}

export default function PageTitle({ title }: Props) {
  return <Heading size={{ base: "2xl", lg: "4xl" }}>{title}</Heading>;
}
