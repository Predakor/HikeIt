import { Select, Portal, type ListCollection } from "@chakra-ui/react";
import type { ControllerRenderProps } from "react-hook-form";

interface Props {
  field: ControllerRenderProps;
  collection: ListCollection;
  placeholder?: string;
}

function ControlledSelect({ field, collection, placeholder }: Props) {
  return (
    <Select.Root
      name={field.name}
      value={field.value}
      onValueChange={({ value }) => field.onChange(value)}
      onInteractOutside={() => field.onBlur()}
      collection={collection}
    >
      <Select.HiddenSelect />
      <Select.Control>
        <Select.Trigger>
          <Select.ValueText placeholder={placeholder} />
        </Select.Trigger>
        <Select.IndicatorGroup>
          <Select.Indicator />
        </Select.IndicatorGroup>
      </Select.Control>
      <Portal>
        <Select.Positioner>
          <Select.Content>
            {collection.items.map((item) => (
              <Select.Item item={item} key={item.id}>
                {item.name}
                <Select.ItemIndicator />
              </Select.Item>
            ))}
          </Select.Content>
        </Select.Positioner>
      </Portal>
    </Select.Root>
  );
}
export default ControlledSelect;
