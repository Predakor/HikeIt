import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { Field } from "@chakra-ui/react";
import { type FieldValues } from "react-hook-form";
import MapEntry from "./MapEntry/MapEntry";
import type { InputsConfig, RenderInputBaseProps } from "./inputTypes";

interface Props<T extends FieldValues> extends RenderInputBaseProps<T> {
  config: InputsConfig;
}

function RenderInputs<T extends FieldValues>(props: Props<T>) {
  const { config, control, register, displayOptions: options } = props;

  const mappedConfig = config.map((entry) => ({
    ...entry,
    label: entry.label ? entry.label : KeyToLabelFormatter(entry.key),
  })) as InputsConfig;

  return mappedConfig.map((entry) => (
    <Field.Root key={entry.key} minWidth={40}>
      <MapEntry
        entry={entry}
        control={control}
        register={register}
        displayOptions={options}
      />
    </Field.Root>
  ));
}

export default RenderInputs;
