import type { Action } from "@/types/Utils/func.types";
import { Button, For, type ButtonProps } from "@chakra-ui/react";

export interface RadioButtonProps<T>
  extends Pick<ButtonProps, "size" | "colorPalette"> {
  onChange: Action<T>;
  selectedItem: T;
  items: { label: string; value: T }[];
}

export function RadioButtons<T>(props: RadioButtonProps<T>) {
  const { items, selectedItem, onChange, ...rest } = props;
  return (
    <For each={items}>
      {({ label, value }) => (
        <Button
          colorPalette={"green"}
          onClick={() => onChange(value)}
          {...rest}
          variant={value === selectedItem ? "solid" : "outline"}
        >
          {label}
        </Button>
      )}
    </For>
  );
}
