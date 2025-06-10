import type { EntryGroup } from "@/types/Utils/OrderTypes";
import MapWrapper from "../../../Wrappers/Mapping/MapWrapper";
import RenderEntry from "../GenericRenderer/GenericRenderer";
import { Card, Center } from "@chakra-ui/react";
import { ToTitleCase } from "@/Utils/ObjectToArray";

interface Props<T> {
  group: EntryGroup<keyof T, T>;
  data: T;
}

export function GroupRenderer<T>({ group, data }: Props<T>) {
  return (
    <Card.Root>
      <Card.Header fontSize={"2xl"}>
        <Center>{ToTitleCase(group.label)}</Center>
      </Card.Header>
      <Card.Body>
        <MapWrapper
          items={group.items}
          Wrapper={group.Wrapper}
          renderItem={(e, i) => <RenderEntry entry={e} data={data} key={i} />}
        />
      </Card.Body>
    </Card.Root>
  );
}
