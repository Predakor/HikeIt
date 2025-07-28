import ControlledSelect from "@/components/RegionSelect/ControlledSelect";
import useCollection from "@/components/RegionSelect/useCollection";
import { Input } from "@chakra-ui/react";
import { Controller, type FieldValues } from "react-hook-form";
import InputLabel from "./MapEntry/FieldWrapper/InputLabel";
import type {
  RenderInputBaseProps,
  SelectInputConfigEntry,
} from "./inputTypes";

interface Props<T extends FieldValues> extends RenderInputBaseProps<T> {
  entry: SelectInputConfigEntry;
  register: undefined;
}

function Select<T extends FieldValues>({
  entry,
  control,
  displayOptions,
}: Props<T>) {
  const [collection] = useCollection(entry.collection);

  if (!collection) {
    return <Input />;
  }

  const inlineLabel = displayOptions?.label === "inline";

  return (
    <>
      <InputLabel label={entry.label} option={displayOptions?.label} />
      <Controller
        control={control}
        name={entry.key}
        render={({ field }) => (
          <ControlledSelect
            field={field}
            collection={collection}
            placeholder={inlineLabel ? entry.label : ""}
          />
        )}
      />
    </>
  );
}

export default Select;
