import type { ReactNode } from "react";
import type { FieldError } from "react-hook-form";
import type { DisplayOptions } from "../../inputTypes";
import InputError from "./InputError";
import InputLabel from "./InputLabel";

interface FieldWrapperProps {
  label: string;
  displayOptions?: DisplayOptions;
  error?: FieldError;
  children: ReactNode;
}

export default function FieldWrapper({
  children,
  label,
  error,
  displayOptions,
}: FieldWrapperProps) {
  return (
    <>
      <InputLabel label={label} option={displayOptions?.label} />
      {children}
      <InputError label={label} error={error} />
    </>
  );
}
