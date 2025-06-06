import { Field, InputGroup, Input } from "@chakra-ui/react";
import type { FieldEntry } from "../../../types/types";

interface Props {
  entry: FieldEntry;
  updateData: (key: string, value: any) => void;
}

function InputField({ entry, updateData }: Props) {
  return (
    <Field.Root>
      <Field.Label>{entry.label}</Field.Label>
      <InputGroup endElement={entry?.unit}>
        <Input
          type={entry.type}
          onChange={(e) => updateData(entry.name, e.target.value)}
        />
      </InputGroup>
    </Field.Root>
  );
}
export default InputField;
