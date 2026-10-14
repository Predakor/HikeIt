import { Field, HStack, Slider } from "@chakra-ui/react";
import { Controller, type FieldValues } from "react-hook-form";
import type {
  RangeInputConfigEntry,
  RenderInputBaseProps,
} from "../../../Utils/RenderInputs/inputTypes";

interface RangeProps<T extends FieldValues> extends Omit<RenderInputBaseProps<T>, "register"> {
  entry: RangeInputConfigEntry<T>;
}

export function Range<T extends FieldValues>({ entry, control, form }: RangeProps<T>) {
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
            value={[field.value ?? entry.min]}
            onValueChange={({ value }) => {
              field.onChange(value[0]);
              if (form) {
                entry.effect?.(value[0], form);
              }
            }}
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
