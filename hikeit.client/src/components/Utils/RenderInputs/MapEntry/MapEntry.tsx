import { PasswordInput } from "@/components/ui/password-input";
import { Input } from "@chakra-ui/react";
import type { FieldError, FieldValues } from "react-hook-form";
import { Range } from "../Range";
import Select from "../Select";
import type { InputConfigEntry, RenderInputBaseProps } from "../inputTypes";
import InputError from "./InputError";
import InputLabel from "./InputLabel";

interface Props<T extends FieldValues> extends RenderInputBaseProps<T> {
  entry: InputConfigEntry;
  error?: FieldError;
}

export default function MapEntry<TFor extends FieldValues>({
  entry,
  control,
  register,
  displayOptions: options,
  error,
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
    return <Range entry={entry} control={control} register={undefined} />;
  }

  if (type === "select") {
    return <Select entry={entry} control={control} register={undefined} />;
  }

  if (type === "checkbox") {
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
      <InputError error={error} />
    </>
  );
}
