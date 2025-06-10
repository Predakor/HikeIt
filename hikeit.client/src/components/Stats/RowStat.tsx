import { HStack, Icon, Stat, Span } from "@chakra-ui/react";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { StatAddons } from "@/types/Utils/StatTypes";

interface Props {
  value: string | number;
  label: string;
  addons?: StatAddons;
}

export default function RowStat({ value, label, addons }: Props) {
  const { IconSource, badge, unit, formatt } = addons ?? {};

  return (
    <Stat.Root>
      <Stat.Label>{ToTitleCase(label)}</Stat.Label>
      <HStack fontSize={"lg"}>
        {IconSource && (
          <Icon size={"md"}>
            <IconSource />
          </Icon>
        )}
        <Stat.ValueText fontSize={"3xl"}>
          {formatt ? formatt(value) : value}
        </Stat.ValueText>
        {unit && <Span alignSelf={"flex-end"}>{unit}</Span>}
        {badge && badge}
      </HStack>
    </Stat.Root>
  );
}
