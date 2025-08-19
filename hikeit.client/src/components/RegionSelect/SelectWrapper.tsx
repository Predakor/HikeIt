import { For, Select, type ListCollection } from "@chakra-ui/react";
import type { ReactElement } from "react";

interface WithId {
  id: string | number;
}

interface Props<T extends WithId> {
  collection: ListCollection<T>;
  onValueChange?: (element: T) => void;
  children: (data: T) => ReactElement;
}

function SelectWrapper<T extends WithId>({
  collection,
  onValueChange,
  children,
}: Props<T>) {
  console.log(collection.items);

  return (
    <Select.Root
      collection={collection}
      onValueChange={
        onValueChange ? (e) => onValueChange(e.items[0]) : undefined
      }
      defaultValue={["1"]}
    >
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
            {(data: T) => {
              console.log(data);

              return (
                <Select.Item item={data} key={data.id}>
                  {data.region.name}
                  <Select.ItemIndicator />
                </Select.Item>
              );
            }}
          </For>
        </Select.Content>
      </Select.Positioner>
    </Select.Root>
  );
}

export default SelectWrapper;
