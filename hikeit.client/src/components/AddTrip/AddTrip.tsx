import { Button, Field, FileUpload, Input, Stack } from "@chakra-ui/react";
import { HiUpload } from "react-icons/hi";

function AddTrip() {
  return (
    <Stack>
      <Field.Root>
        <Field.Label>Date</Field.Label>
        <Input type="date" />
      </Field.Root>

      <Field.Root>
        <Field.Label>Gpx file</Field.Label>
        <FileUpload.Root accept={".gpx"}>
          <FileUpload.HiddenInput />
          <FileUpload.Trigger asChild>
            <Button variant="outline" size="sm">
              <HiUpload /> Upload file
            </Button>
          </FileUpload.Trigger>
          <FileUpload.List />
        </FileUpload.Root>
      </Field.Root>

      <Field.Root>
        <Field.Label>Region</Field.Label>
      </Field.Root>
      <Button />
    </Stack>
  );
}

export default AddTrip;
