import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import StatGroup from "@/components/Stats/StatGroup";
import type { StatType } from "@/components/Stats/_typesStats";
import type { EntryGroup, EntryItem } from "@/types/Utils/OrderTypes";
import { Card, Center } from "@chakra-ui/react";
import MapWrapper from "../../../Wrappers/Mapping/MapWrapper";
import RenderEntry from "../GenericRenderer/GenericRenderer";

interface Props<T> {
  group: EntryGroup<keyof T, T>;
  data: T;
}

export function GroupRenderer<T>({ data, group }: Props<T>) {
  const nestedGroups = group.items.filter((s) => s.type === "group");
  const mappedStats = group.items
    .filter((i): i is EntryItem<keyof T, T> => i.type !== "group")
    .map((item) => {
      const key = item.key;
      const itemData = data[key];

      const itemLabel = item.label
        ? item.label
        : KeyToLabelFormatter(key as string);

      return {
        ...item,
        label: itemLabel,
        value: itemData as string | number,
      };
    })
    .filter((v) => v !== null) as StatType[];

  return (
    <Card.Root>
      <Card.Header fontSize={"2xl"}>
        <Center>{ToTitleCase(group.label)}</Center>
      </Card.Header>
      <Card.Body>
        <StatGroup stats={mappedStats} />
        <MapWrapper
          items={nestedGroups}
          Wrapper={group.Wrapper}
          renderItem={(e, i) => <RenderEntry entry={e} data={data} key={i} />}
        />
      </Card.Body>
    </Card.Root>
  );
}
