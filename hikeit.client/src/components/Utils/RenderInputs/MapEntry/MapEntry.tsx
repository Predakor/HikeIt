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
  const { key, label, type } = entry;

  const inlineLabel = options?.label === "inline" ? label : "";

  const wrapperConfig = {
    label: entry.label,
    error: error,
    displayOptions: options,
  };

  const inputConfig = {
    label,
    type,
    placeholder: inlineLabel,
    size: options?.size,
  };

  const validationConfig = register(key, {
    ...entry,
  });

  switch (type) {
    case "range":
      return <Range entry={entry} control={control} register={undefined} />;

    case "select":
      return <Select entry={entry} control={control} register={undefined} />;

    case "checkbox":
      return "not impleneted checkbox type";

    case "password":
      return (
        <FieldWrapper {...wrapperConfig}>
          <PasswordInput {...inputConfig} {...validationConfig} />
        </FieldWrapper>
      );

    default:
      return (
        <FieldWrapper {...wrapperConfig}>
          <Input {...inputConfig} {...validationConfig} />
        </FieldWrapper>
      );
  }
}
