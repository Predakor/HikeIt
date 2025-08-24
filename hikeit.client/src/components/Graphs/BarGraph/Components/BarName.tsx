import { Span } from "@chakra-ui/react";
import type { BaseProps } from "./BarValue";

export function BarName({ item }: BaseProps) {
  return (
    <Span truncate minH={"fit-content"} fontWeight={"medium"} fontSize={"sm"}>
      {item.name}
    </Span>
  );
}
