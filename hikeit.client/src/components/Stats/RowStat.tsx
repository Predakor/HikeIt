import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { StatAddons } from "@/types/Utils/stat.types";
import { Icon, Stack, Stat, type StatRootProps } from "@chakra-ui/react";

interface Props<T> {
  value: T;
  label: string;
  addons?: StatAddons<T>;
  styles?: Partial<StatRootProps>;
}

export default function RowStat<T>({ value, label, addons, styles }: Props<T>) {
  const { IconSource, badge, unit, formatt } = addons ?? {};

  const statValue = formatt ? formatt(value as T) : value;
  return (
    <Stat.Root alignItems={{ base: "start", lg: "center" }} {...styles}>
      <Stat.Label>{ToTitleCase(label)}</Stat.Label>
      <Stack direction={"row"} alignItems={"baseline"} fontSize={"lg"}>
        {IconSource && (
          <Icon size={"md"}>
            <IconSource />
          </Icon>
        )}
        <Stat.ValueText fontSize={"3xl"}>{`${statValue}`}</Stat.ValueText>
        {unit && <Stat.ValueUnit fontSize={"xl"}>{unit}</Stat.ValueUnit>}
        {badge && badge}
      </Stack>
    </Stat.Root>
  );
}
export { RowStat };
