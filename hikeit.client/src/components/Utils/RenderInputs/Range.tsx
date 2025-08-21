import { Field, HStack, Slider } from "@chakra-ui/react";
import { Controller } from "react-hook-form";
import type { RangeInputConfigEntry, RenderInputBaseProps } from "./inputTypes";

interface RangeProps<T> extends Omit<RenderInputBaseProps<T>, "register"> {
  entry: RangeInputConfigEntry;
}

export function Range<T>({ entry, control }: RangeProps<T>) {
  const { key, label } = entry;
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
            onValueChange={({ value }) => field.onChange(value[0])}
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
