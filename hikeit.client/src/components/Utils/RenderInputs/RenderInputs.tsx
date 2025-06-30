import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { Field } from "@chakra-ui/react";
import { type FieldValues, type UseFormReturn } from "react-hook-form";
import type { DisplayOptions, InputsConfig } from "./inputTypes";
import MapEntry from "./MapEntry/MapEntry";

interface Props<T extends FieldValues> {
  config: InputsConfig;
  formHook: UseFormReturn<T, any, T>;
  displayOptions?: DisplayOptions;
}

function RenderInputs<T extends FieldValues>(props: Props<T>) {
  const { config, formHook, displayOptions: options } = props;

  const {
    control,
    register,
    formState: { errors },
  } = formHook;

  const mappedConfig = config.map((entry) => ({
    ...entry,
    label: entry.label ? entry.label : KeyToLabelFormatter(entry.key),
  })) as InputsConfig;

  return mappedConfig.map((entry) => (
    <Field.Root key={entry.key} minWidth={40} invalid={!!errors[entry.key]}>
      <MapEntry
        entry={entry}
        control={control}
        register={register}
        displayOptions={options}
        error={errors[entry.key] as any}
      />
    </Field.Root>
  ));
}

export default RenderInputs;
