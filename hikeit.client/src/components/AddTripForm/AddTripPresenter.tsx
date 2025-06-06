import { Field, Input, InputGroup, Stack } from "@chakra-ui/react";
import { type UseFormReturn } from "react-hook-form";
import RegionSelect from "../RegionSelect/RegionSelect";
import { tripFormConfig } from "./AddTrip/data";
import type { TripDto } from "@/types/ApiTypes/TripDtos";

interface Props {
  formHandler: UseFormReturn<TripDto>;
}

function AddTripPresenter({ formHandler }: Props) {
  const { register, setValue } = formHandler;

  return (
    <Stack>
      {tripFormConfig.map((entry) => (
        <Field.Root key={entry.name}>
          <Field.Label>{entry.label}</Field.Label>
          <InputGroup endElement={entry.unit}>
            <Input
              type={entry.type}
              {...register(entry.name as keyof TripDto)}
            />
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
