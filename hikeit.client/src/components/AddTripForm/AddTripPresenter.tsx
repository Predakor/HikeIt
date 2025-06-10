import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { Field, Input, InputGroup, Stack } from "@chakra-ui/react";
import { type UseFormReturn } from "react-hook-form";
import RegionSelect from "../RegionSelect/RegionSelect";
import { tripFormConfig } from "./AddTrip/data";
import { ToTitleCase } from "@/Utils/ObjectToArray";

interface Props {
  formHandler: UseFormReturn<TripDto>;
}

function AddTripPresenter({ formHandler }: Props) {
  const { register, setValue } = formHandler;

  return (
    <Stack>
      {tripFormConfig.map(({ label, name, type, unitAddods }) => (
        <Field.Root key={name}>
          <Field.Label>{ToTitleCase(label)}</Field.Label>
          <InputGroup endElement={unitAddods?.unit ?? null}>
            <Input type={type} {...register(`base.${name}`)} />
          </InputGroup>
        </Field.Root>
      ))}

      <Field.Root>
        <Field.Label>Region</Field.Label>
        <RegionSelect
          onValueChange={(region) => setValue("regionId", region.id)}
        />
      </Field.Root>
    </Stack>
  );
}

export default AddTripPresenter;
