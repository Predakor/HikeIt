import { PasswordInput } from "@/components/ui/password-input";
import { Field, Input, VisuallyHidden } from "@chakra-ui/react";
import type { FieldValues } from "react-hook-form";
import { Range } from "../Range";
import type { InputConfigEntry, RenderInputBaseProps } from "../inputTypes";

interface Props<T extends FieldValues> extends RenderInputBaseProps<T> {
  entry: InputConfigEntry;
}

export default function MapEntry<TFor extends FieldValues>({
  entry,
  control,
  register,
  displayOptions: options,
}: Props<TFor>) {
  const { key, label, type, required } = entry;

  const inlineLabel = options?.label === "inline" ? label : "";
  const shared = {
    label,
    type,
    placeholder: inlineLabel,
    size: options?.size,
  };

  if (type === "range") {
    return <Range entry={entry} control={control} />;
  }

  if (type === "checkbox") {
    return "not impleneted checkbox type";
  }

  if (type === "date") {
    return "not impleneted checkbox type";
  }

  if (type === "password") {
    return (
      <>
        <InputLabel label={label} option={options?.label} />
        <PasswordInput
          {...shared}
          {...register(key, {
            minLength: entry.min,
            max: entry.max,
            pattern: entry.pattern,
            required,
          })}
        />
      </>
    );
  }

  return (
    <>
      <InputLabel label={label} option={options?.label} />
      <Input
        {...shared}
        {...register(key, {
          minLength: entry.min,
          maxLength: entry.max,
          required,
        })}
      />
    </>
  );
}

interface InputLabelProps {
  label: string;
  option: "inline" | "ontop" | undefined;
}

function InputLabel({ label, option }: InputLabelProps) {
  if (option === "inline") {
    return (
      <VisuallyHidden>
        <Field.Label>{label}</Field.Label>
      </VisuallyHidden>
    );
  }

  return <Field.Label>{label}</Field.Label>;
}
