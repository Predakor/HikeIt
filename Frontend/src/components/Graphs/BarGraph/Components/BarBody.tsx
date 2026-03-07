import { Box } from "@chakra-ui/react";
import type { BaseProps } from "./BarValue";

export function BarBody({ item }: BaseProps) {
  const color = (item.color ?? "blue") as string;
  return (
    <Box
      boxShadow={"2xl"}
      boxShadowColor={color}
      backgroundColor={color}
      borderRadius={"xs"}
      flexGrow={1}
      minH={"8"}
      height={"100%"}
    />
  );
}
