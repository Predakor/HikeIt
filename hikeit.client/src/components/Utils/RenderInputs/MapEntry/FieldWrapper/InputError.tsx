import type { FullMap } from "@/types/Utils/MappingTypes";
import { Field } from "@chakra-ui/react";
import type { FieldError, RegisterOptions } from "react-hook-form";

interface Props {
  error: FieldError | undefined;
  label: string;
}

export default function InputError({ error, label }: Props) {
  if (!error || !error?.type) {
    return;
  }

  const key = error.type as keyof typeof errorMap;
  const errorType = errorMap[key];

  return (
    <Field.ErrorText>
      {label} {errorType} {error?.message || ""}
    </Field.ErrorText>
  );
}

const errorMap: FullMap<RegisterOptions, string> = {
  minLength: "Too Short",
  maxLength: "Too Long",
  pattern: "Must include",
  required: "Field can't be empty",
  min: "Must be greater than",
  max: "Must be smaller than",
  validate: "",
  value: "",
  setValueAs: "",
  shouldUnregister: "",
  onChange: "",
  onBlur: "",
  disabled: "",
  deps: "",
  valueAsNumber: "",
  valueAsDate: "",
};
