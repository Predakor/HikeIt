import { Field, HStack, Slider } from "@chakra-ui/react";
import { useController, type FieldValues } from "react-hook-form";
import type {
  IntervalInputConfigEntry,
  RenderInputBaseProps,
} from "../../../Utils/RenderInputs/inputTypes";

interface IntervalProps<T extends FieldValues> extends Omit<RenderInputBaseProps<T>, "register"> {
  entry: IntervalInputConfigEntry;
}

export function Interval<T extends FieldValues>({ entry, control }: IntervalProps<T>) {
  const { key, maxKey, label, min, max, step } = entry;
  const { field: lower } = useController({ control, name: key as any });
  const { field: upper } = useController({ control, name: maxKey as any });

  const value = [lower.value ?? min, upper.value ?? max];

  return (
    <>
      <HStack>
        <Field.Label>{label}</Field.Label>
        {value[0]} - {value[1]}
      </HStack>
      <Slider.Root
        value={value}
        onValueChange={({ value: [from, to] }) => {
          lower.onChange(from);
          upper.onChange(to);
        }}
        width={"full"}
        min={min}
        max={max}
        step={step}
      >
        <Slider.Control>
          <Slider.Track>
            <Slider.Range />
          </Slider.Track>
          <Slider.Thumbs />
        </Slider.Control>
      </Slider.Root>
    </>
  );
}
