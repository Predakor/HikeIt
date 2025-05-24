import { Field, InputGroup, Input, Button, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import RegionSelect from "../RegionSelect/RegionSelect";
import { AddFile } from "./AddFile/AddFile";
import { tripFormConfig } from "./AddTrip/data";

function AddTripForm() {
  const { register, handleSubmit, setValue, watch } = useForm<TripDto>({
    defaultValues: {
      height: 0,
      distance: 0,
      duration: 0,
      regionId: 0,
      tripDay: "",
    },
  });

  const onSubmit = (data: TripDto) => {
    console.log("Submit:", data);
  };

  return (
    <Stack as={"form"} onSubmit={handleSubmit(onSubmit)}>
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

      <AddFile onFileChange={(f) => console.log(f)} />

      <Button type="submit">Upload</Button>
    </Stack>
  );
}

export default AddTripForm;
