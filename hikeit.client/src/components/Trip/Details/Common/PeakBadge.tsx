import { Badge } from "@chakra-ui/react";

interface Props {
  text: string;
  color?: "orange" | "yellow" | "green" | "teal" | "blue";
}

export function PeakBadge({ text, color }: Props) {
  return (
    <Badge
      size={{ base: "xs", lg: "lg" }}
      colorPalette={color || "blue"}
      truncate
    >
      {text}
    </Badge>
  );
}
