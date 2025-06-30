import { Field } from "@chakra-ui/react";
import type { FieldError } from "react-hook-form";

interface Props {
  error: FieldError | undefined;
}

export default function InputError({ error }: Props) {
  if (!error) {
    return;
  }

  return <Field.ErrorText>{error.message || error.type} </Field.ErrorText>;
}
