import { Field, VisuallyHidden } from "@chakra-ui/react";

interface InputLabelProps {
  label: string;
  option: "inline" | "ontop" | undefined;
}

export default function InputLabel({ label, option }: InputLabelProps) {
  if (option === "inline") {
    return (
      <VisuallyHidden>
        <Field.Label>{label}</Field.Label>
      </VisuallyHidden>
    );
  }

  return <Field.Label>{label}</Field.Label>;
}
