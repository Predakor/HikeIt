import { ObjectToArray } from "@/Utils/ObjectToArray";
import { Field, Input } from "@chakra-ui/react";

interface Props {
  label: string;
  value: string | number;
}

export function MapStats({ stats }: { stats: object }) {
  const mappedObject = ObjectToArray(stats);

  return mappedObject.map(([key, data]) => (
    <InfoField label={key} value={data} />
  ));
}

function InfoField({ label, value }: Props) {
  return (
    <Field.Root width={{ base: "full", md: "md" }}>
      <Field.Label fontSize={"md"} color={"GrayText"}>
        {label}
      </Field.Label>
      <Input
        type="text"
        value={value}
        readOnly={true}
        size={{ base: "md", lg: "lg" }}
        variant={"flushed"}
      />
    </Field.Root>
  );
}
