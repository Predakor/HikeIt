import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { Field, Input } from "@chakra-ui/react";
import {
  type Control,
  type FieldValues,
  type Path,
  type UseFormRegister,
} from "react-hook-form";
import { Range } from "./Range";
import type { InputConfigEntry, InputsConfig } from "./inputTypes";

interface Props<TFor extends FieldValues> {
  config: InputsConfig;
  control: Control;
  register: UseFormRegister<TFor>;
}

function RenderInputs<T extends FieldValues>({
  config,
  control,
  register,
}: Props<T>) {
  const mappedConfig = config.map((entry) => ({
    ...entry,
    label: !!entry.label || KeyToLabelFormatter(entry.key),
  })) as InputsConfig;

  return mappedConfig.map((entry) => (
    <Field.Root key={entry.key} w={40}>
      <MapEntry entry={entry} control={control} register={register} />
    </Field.Root>
  ));
}

interface MapProps<TFor extends FieldValues> {
  entry: InputConfigEntry;
  control: Control;
  register: UseFormRegister<TFor>;
}

function MapEntry<TFor extends FieldValues>({
  entry,
  control,
  register,
}: MapProps<TFor>) {
  const { key, label, type } = entry;

  if (type === "range") {
    return <Range entry={entry} control={control} />;
  }

  return (
    <>
      <Field.Label>{label}</Field.Label>
      <Input type={type} {...register(key as Path<TFor>)} />
    </>
  );
}

export default RenderInputs;
