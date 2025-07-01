import { IconPeak } from "@/Icons/Icons";
import { Icon } from "@chakra-ui/react";

export function PeakIcon() {
  return (
    <Icon hideBelow={"lg"} fontSize={{ base: 32, md: 40, xl: 52 }}>
      <IconPeak />
    </Icon>
  );
}
