import { HStack, Icon, Stat, Span } from "@chakra-ui/react";
import type { StatAddons } from "../Trip/Data/ValueMap";
import { ToTitleCase } from "@/Utils/ObjectToArray";

interface Props {
  value: string | number;
  label: string;
  addons?: StatAddons;
}

export default function RowStat({ value, label, addons }: Props) {
  const { IconSource, badge, unit } = addons ?? {};
  return (
    <Stat.Root>
      <Stat.Label>{ToTitleCase(label)}</Stat.Label>
      <HStack fontSize={"lg"}>
        {IconSource && (
          <Icon size={"md"}>
            <IconSource />
          </Icon>
        )}
        <Stat.ValueText fontSize={"3xl"}>{value}</Stat.ValueText>
        {unit && <Span alignSelf={"flex-end"}>{unit}</Span>}
        {badge && badge}
      </HStack>
    </Stat.Root>
  );
}
