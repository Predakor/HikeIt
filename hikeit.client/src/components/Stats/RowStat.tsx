import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { StatAddons } from "@/types/Utils/StatTypes";
import { HStack, Icon, Stat } from "@chakra-ui/react";

interface Props {
  value: string | number;
  label: string;
  addons?: StatAddons;
}

export default function RowStat({ value, label, addons }: Props) {
  const { IconSource, badge, unit, formatt } = addons ?? {};

  const statValue = formatt ? formatt(value) : value;
  return (
    <Stat.Root alignItems={{ base: "start", lg: "center" }}>
      <Stat.Label>{ToTitleCase(label)}</Stat.Label>
      <HStack alignItems={"baseline"} fontSize={"lg"}>
        {IconSource && (
          <Icon size={"md"}>
            <IconSource />
          </Icon>
        )}
        <Stat.ValueText fontSize={"3xl"}>{statValue}</Stat.ValueText>
        {unit && <Stat.ValueUnit fontSize={"xl"}>{unit}</Stat.ValueUnit>}
        {badge && badge}
      </HStack>
    </Stat.Root>
  );
}
