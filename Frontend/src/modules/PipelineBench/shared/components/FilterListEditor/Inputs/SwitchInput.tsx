import { Switch } from "@chakra-ui/react";
import { fieldLabel, type FilterFieldValue } from "../filterCatalog";

export interface BooleanField {
  kind: "boolean";
  name: string;
  defaultValue: boolean;
}

interface SwitchInputProps {
  field: BooleanField;
  value: FilterFieldValue | undefined;
  onValueChange: (value: boolean) => void;
}

export function SwitchInput({ field, value, onValueChange }: SwitchInputProps) {
  return (
    <Switch.Root
      size={"sm"}
      checked={value === true}
      onCheckedChange={({ checked }) => onValueChange(checked)}
    >
      <Switch.HiddenInput />
      <Switch.Control />
      <Switch.Label>{fieldLabel(field.name)}</Switch.Label>
    </Switch.Root>
  );
}
