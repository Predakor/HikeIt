import { PasswordInput } from "@/components/ui/password-input";
import { Input } from "@chakra-ui/react";
import type { FieldError, FieldValues } from "react-hook-form";
import { Range } from "../Range";
import Select from "../Select";
import type { InputConfigEntry, RenderInputBaseProps } from "../inputTypes";
import FieldWrapper from "./FieldWrapper/FieldWrapper";

interface FieldWrapper<T extends FieldValues> extends RenderInputBaseProps<T> {
  entry: InputConfigEntry;
  error?: FieldError;
}

export default function MapEntry<TFor extends FieldValues>({
  entry,
  control,
  register,
  displayOptions: options,
  error,
}: FieldWrapper<TFor>) {
  const { key, label, type, required } = entry;

  const inlineLabel = options?.label === "inline" ? label : "";

  const wrapperShared = {
    label: entry.label,
    error: error,
    displayOptions: options,
  };

  const shared = {
    label,
    type,
    placeholder: inlineLabel,
    size: options?.size,
  };

  console.log(type);

  switch (type) {
    case "range":
      return <Range entry={entry} control={control} register={undefined} />;

    case "select":
      return <Select entry={entry} control={control} register={undefined} />;

    case "checkbox":
      return "not impleneted checkbox type";

    default:
      <FieldWrapper {...wrapperShared}>
        <Input
          {...shared}
          {...register(key, {
            minLength: entry.min,
            max: entry.max,
            required,
          })}
        />
      </FieldWrapper>;
      return;

    case "date":
      return (
        <FieldWrapper {...wrapperShared}>
          <Input
            {...shared}
            {...register(key, {
              minLength: entry.min,
              max: entry.max,
              required,
            })}
          />
        </FieldWrapper>
      );

    case "password":
      return (
        <FieldWrapper {...wrapperShared}>
          <PasswordInput
            {...shared}
            {...register(key, {
              minLength: entry.min,
              max: entry.max,
              pattern: entry.pattern,
              required,
            })}
          />
        </FieldWrapper>
      );

    case "text":
      return (
        <FieldWrapper {...wrapperShared}>
          <Input
            {...shared}
            {...register(key, {
              minLength: entry.min,
              max: entry.max,
              pattern: entry.pattern,
              required,
            })}
          />
        </FieldWrapper>
      );
  }
}
