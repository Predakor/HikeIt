import { For, Select, type ListCollection } from "@chakra-ui/react";
import type { ReactElement } from "react";

interface WithId {
  id: string | number;
}

interface Props<T extends WithId> {
  collection: ListCollection<T>;
  children: (data: T) => ReactElement;
}

function SelectWrapper<T extends WithId>({ collection, children }: Props<T>) {
  return (
    <Select.Root collection={collection}>
      <Select.HiddenSelect />
      <Select.Label />
      <Select.Control>
        <Select.Trigger>
          <Select.ValueText />
        </Select.Trigger>
        <Select.IndicatorGroup>
          <Select.Indicator />
          <Select.ClearTrigger />
        </Select.IndicatorGroup>
      </Select.Control>
      <Select.Positioner>
        <Select.Content>
          <For each={collection.items}>
            {(data) => (
              <Select.Item item={data} key={data.id}>
                {children(data)}
                <Select.ItemIndicator />
              </Select.Item>
            )}
          </For>
        </Select.Content>
      </Select.Positioner>
    </Select.Root>
  );
}

export default SelectWrapper;
