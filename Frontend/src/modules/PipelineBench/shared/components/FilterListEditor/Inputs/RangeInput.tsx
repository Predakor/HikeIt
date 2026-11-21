import { Flex, Slider, Text } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { fieldLabel } from "../filterCatalog";
import type { NumberField } from "./NumberInput";

interface RangeInputProps {
  field: NumberField;
  value: number;
  onValueChange: (value: number) => void;
}

// Same Chakra slider parts as the shared Range input, driven by value/onChange since this editor is not a react-hook-form.
// The dragged value lives in local state and is committed when the drag ends, so a drag does not re-render the whole bench per tick.
export function RangeInput({ field, value, onValueChange }: RangeInputProps) {
  const [draft, setDraft] = useState(value);

  useEffect(() => {
    setDraft(value);
  }, [value]);

  return (
    <Flex align={"center"} gap={2}>
      <Text fontSize={"xs"} color={"fg.muted"}>
        {fieldLabel(field.name)}
      </Text>
      <Slider.Root
        width={"28"}
        min={field.min}
        max={field.max}
        step={field.step}
        value={[draft]}
        onValueChange={({ value: next }) => setDraft(next[0])}
        onValueChangeEnd={({ value: next }) => onValueChange(next[0])}
      >
        <Slider.Control>
          <Slider.Track>
            <Slider.Range />
          </Slider.Track>
          <Slider.Thumbs />
        </Slider.Control>
      </Slider.Root>
      <Text fontSize={"xs"} minW={"10"} textAlign={"right"}>
        {draft}
      </Text>
    </Flex>
  );
}
