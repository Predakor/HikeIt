import { PasswordInput } from "@/components/ui/Inputs/Password/password-input";
import { Input } from "@chakra-ui/react";
import type { FieldError, FieldValues, Path } from "react-hook-form";
import { Interval } from "../../../ui/Inputs/Interval/Interval";
import { Range } from "../../../ui/Inputs/Range/Range";
import Select from "../../../ui/Inputs/Select/Select";
import { Toggle } from "../../../ui/Inputs/Toggle/Toggle";
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

  const inputProps = {
    required: entry.required,
    min: "min" in entry ? entry.min : undefined,
    max: "max" in entry ? entry.max : undefined,
    step: "step" in entry ? entry.step : undefined,
  };

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

  const validationConfig = register(key as Path<TFor>, {
    ...entry,
  });

  switch (type) {
    case "range":
      return <Range entry={entry} control={control} />;

    case "interval":
      return <Interval entry={entry} control={control} />;

    case "select":
      return <Select entry={entry} control={control} />;

    case "checkbox":
      return <Toggle entry={entry} control={control} />;

    case "password":
      return (
        <FieldWrapper {...wrapperConfig}>
          <PasswordInput {...inputConfig} {...validationConfig} />
        </FieldWrapper>
      );

    default:
      return (
        <FieldWrapper {...wrapperConfig}>
          <Input {...inputProps} {...inputConfig} {...validationConfig} />
        </FieldWrapper>
      );
  }
}
