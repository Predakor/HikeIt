import { Flex, Input, Text } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { fieldLabel } from "../filterCatalog";

// `control` picks the widget: "range" slider for small bounded values, "number" input for wide ranges.
export interface NumberField {
  kind: "number";
  name: string;
  control: "range" | "number";
  min: number;
  max: number;
  step: number;
  defaultValue: number;
}

interface NumberInputProps {
  field: NumberField;
  value: number;
  onValueChange: (value: number) => void;
}

// Keeps the raw text while typing ("1." is not a number yet), so the input is not reset mid-edit.
export function NumberInput({ field, value, onValueChange }: NumberInputProps) {
  const [draft, setDraft] = useState(String(value));

  useEffect(() => {
    setDraft((previous) => (Number(previous) === value ? previous : String(value)));
  }, [value]);

  const handleChange = (raw: string) => {
    setDraft(raw);

    const parsed = Number(raw);
    const isValidNumber = raw !== "" && !Number.isNaN(parsed);

    if (isValidNumber) {
      onValueChange(parsed);
    }
  };

  return (
    <Flex align={"center"} gap={1}>
      <Text fontSize={"xs"} color={"fg.muted"}>
        {fieldLabel(field.name)}
      </Text>
      <Input
        type={"number"}
        size={"xs"}
        w={"20"}
        min={field.min}
        max={field.max}
        step={field.step}
        value={draft}
        onChange={(event) => handleChange(event.target.value)}
      />
    </Flex>
  );
}
