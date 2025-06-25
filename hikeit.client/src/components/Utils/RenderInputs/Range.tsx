import { Field, HStack, Slider } from "@chakra-ui/react";
import { Controller, type Control } from "react-hook-form";
import type { RangeInputConfigEntry } from "./inputTypes";

interface RangeProps {
  entry: RangeInputConfigEntry;
  control: Control<any, any, any>;
}

export function Range({ entry, control }: RangeProps) {
  const { key, label } = entry;

  const formatNumber = (value: number) => {
    if (entry.formatValue) {
      return entry.formatValue(value);
    }
    return value;
  };

  return (
    <Controller
      name={key}
      control={control}
      render={({ field }) => (
        <>
          <HStack>
            <Field.Label>{label}</Field.Label>
            {field.value && field.value}
          </HStack>
          <Slider.Root
            onValueChange={({ value }) =>
              field.onChange(formatNumber(value[0]))
            }
            width={"full"}
            max={entry.max}
            min={entry.min}
          >
            <Slider.Control>
              <Slider.Track>
                <Slider.Range />
              </Slider.Track>
              <Slider.Thumbs />
            </Slider.Control>
          </Slider.Root>
        </>
      )}
    />
  );
}
