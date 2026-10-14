import { Switch } from "@chakra-ui/react";
import { Controller, type FieldValues, type Path } from "react-hook-form";
import type {
  CheckboxInputConfigEntry,
  RenderInputBaseProps,
} from "../../../Utils/RenderInputs/inputTypes";

interface ToggleProps<T extends FieldValues> extends Omit<RenderInputBaseProps<T>, "register"> {
  entry: CheckboxInputConfigEntry;
}

export function Toggle<T extends FieldValues>({ entry, control }: ToggleProps<T>) {
  return (
    <Controller
      name={entry.key as Path<T>}
      control={control}
      render={({ field }) => (
        <Switch.Root
          checked={!!field.value}
          onCheckedChange={({ checked }) => field.onChange(checked)}
        >
          <Switch.HiddenInput />
          <Switch.Control />
          <Switch.Label>{entry.label}</Switch.Label>
        </Switch.Root>
      )}
    />
  );
}
