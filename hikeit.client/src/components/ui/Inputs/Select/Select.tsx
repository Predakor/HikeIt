import ControlledSelect from "@/components/ui/Inputs/Select/ControlledSelect";
import useCollection from "@/components/ui/Inputs/Select/useCollection";
import { Input } from "@chakra-ui/react";
import { Controller, type FieldValues } from "react-hook-form";
import InputLabel from "../../../Utils/RenderInputs/MapEntry/FieldWrapper/InputLabel";
import type {
  RenderInputBaseProps,
  SelectInputConfigEntry,
} from "../../../Utils/RenderInputs/inputTypes";

interface Props<T extends FieldValues>
  extends Omit<RenderInputBaseProps<T>, "register"> {
  entry: SelectInputConfigEntry;
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
            displayOptions={displayOptions}
          />
        )}
      />
    </>
  );
}

export default Select;
