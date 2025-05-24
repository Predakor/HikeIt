import { For, Select } from "@chakra-ui/react";
import { WithId, Props } from "./SelectWrapper";

export function SelectWrapper<T extends WithId>({
  collection,
  onValueChange,
  children,
}: Props<T>) {
  return (
    <Select.Root
      collection={collection}
      onValueChange={onValueChange ? (e) => console.log(e.items[0]) : undefined}
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
