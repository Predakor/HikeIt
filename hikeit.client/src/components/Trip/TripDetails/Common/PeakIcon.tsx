import { Icon } from "@chakra-ui/react";
import { GiMountaintop } from "react-icons/gi";

export function PeakIcon() {
  return (
    <Icon hideBelow={"lg"} fontSize={{ base: 32, md: 40, xl: 52 }}>
      <GiMountaintop />
    </Icon>
  );
}
