import type { FilterField, FilterFieldValue } from "./filterCatalog";
import { NumberInput } from "./Inputs/NumberInput";
import { RangeInput } from "./Inputs/RangeInput";
import { SwitchInput } from "./Inputs/SwitchInput";
import { TargetsInput } from "./Inputs/TargetsInput";

interface FieldInputProps {
  field: FilterField;
  value: FilterFieldValue | undefined;
  onValueChange: (value: FilterFieldValue) => void;
}

export function FieldInput({ field, value, onValueChange }: FieldInputProps) {
  if (field.kind === "boolean") {
    return <SwitchInput field={field} value={value} onValueChange={onValueChange} />;
  }

  if (field.kind === "pointFields") {
    return <TargetsInput field={field} value={value} onValueChange={onValueChange} />;
  }

  if (field.control === "range") {
    return <RangeInput field={field} value={Number(value)} onValueChange={onValueChange} />;
  }

  return <NumberInput field={field} value={Number(value)} onValueChange={onValueChange} />;
}
