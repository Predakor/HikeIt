import { Field, HStack, Slider } from "@chakra-ui/react";
import { Controller } from "react-hook-form";
import type { RangeInputConfigEntry, RenderInputBaseProps } from "./inputTypes";

interface RangeProps<T> extends RenderInputBaseProps<T> {
  entry: RangeInputConfigEntry;
}

export function Range<T>({ entry, control }: RangeProps<T>) {
  const { key, label } = entry;

  const formatNumber = (value: number) => {
    if (entry.formatValue) {
      return entry.formatValue(value);
    }
    return value;
  };

  const formatLabel = (value: number) => {
    if (entry.formatLabel) {
      return entry.formatLabel(value);
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
            {field.value && formatLabel(field.value)}
          </HStack>
          <Slider.Root
            onValueChange={({ value }) =>
              field.onChange(formatNumber(value[0]))
            }
            width={"full"}
            max={entry.max}
            min={entry.min}
            step={entry.step}
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
