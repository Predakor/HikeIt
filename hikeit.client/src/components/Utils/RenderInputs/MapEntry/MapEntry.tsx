import { PasswordInput } from "@/components/ui/password-input";
import { Input } from "@chakra-ui/react";
import type { FieldValues } from "react-hook-form";
import { Range } from "../Range";
import type { InputConfigEntry, RenderInputBaseProps } from "../inputTypes";
import InputLabel from "./InputLabel";
import Select from "../Select";

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
    </>
  );
}
