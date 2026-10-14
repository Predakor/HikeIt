import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { Field, Flex, Stack, Text } from "@chakra-ui/react";
import { type FieldError, type FieldValues, type UseFormReturn } from "react-hook-form";
import type { DisplayOptions, InputConfigEntry, InputsLayout } from "./inputTypes";
import MapEntry from "./MapEntry/MapEntry";

interface Props<T extends FieldValues> {
  config: InputsLayout;
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

  const renderLayout = (layout: InputsLayout) =>
    layout.map((item) => {
      if (item.type === "group") {
        const Container = item.direction === "row" ? Flex : Stack;

        return (
          <Stack key={item.key} gap={1}>
            {item.label && <Text fontWeight={"medium"}>{item.label}</Text>}
            <Container wrap={"wrap"} gap={4}>
              {renderLayout(item.items)}
            </Container>
          </Stack>
        );
      }

      const entry = {
        ...item,
        label: item.label ? item.label : KeyToLabelFormatter(item.key),
      } as InputConfigEntry;

      return (
        <Field.Root key={entry.key} minWidth={40} invalid={!!errors[entry.key]}>
          <MapEntry
            {...entry}
            entry={entry}
            key={entry.key}
            control={control}
            register={register}
            displayOptions={options}
            error={errors[entry.key] as FieldError}
          />
        </Field.Root>
      );
    });

  return renderLayout(config);
}

export default RenderInputs;
