import {
  Select,
  Portal,
  type ListCollection,
  type ConditionalValue,
} from "@chakra-ui/react";
import type { ControllerRenderProps } from "react-hook-form";
import type { DisplayOptions } from "../Utils/RenderInputs/inputTypes";

interface Props {
  field: ControllerRenderProps;
  collection: ListCollection;
  placeholder?: string;
  displayOptions?: DisplayOptions;
}

function ControlledSelect({
  field,
  collection,
  placeholder,
  displayOptions,
}: Props) {
  return (
    <Select.Root
      name={field.name}
      value={field.value}
      onValueChange={({ value }) => field.onChange(value)}
      onInteractOutside={() => field.onBlur()}
      collection={collection}
      size={mapToSizes(displayOptions)}
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

const mapToSizes = (
  displayOptions?: DisplayOptions
): ConditionalValue<"sm" | "md" | "lg" | "xs" | undefined> => {
  const size = displayOptions?.size;

  if (!size) {
    return "lg";
  }

  if (size == "md") {
    return "md";
  }

  if (size == "2xl" || size == "xl") {
    return "lg";
  }

  return size;
};
export default ControlledSelect;
