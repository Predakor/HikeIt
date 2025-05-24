import { Button, Field, FileUpload } from "@chakra-ui/react";
import { HiUpload } from "react-icons/hi";

interface AddFileProps {
  onFileChange: (file: File) => void;
}

export function AddFile({ onFileChange }: AddFileProps) {
  return (
    <Field.Root>
      <Field.Label>Gpx file</Field.Label>
      <FileUpload.Root
        accept={".gpx"}
        onFileChange={(f) => onFileChange(f.acceptedFiles[0])}
      >
        <FileUpload.HiddenInput />
        <FileUpload.Trigger asChild>
          <Button variant="outline" size="sm">
            <HiUpload /> Upload file
          </Button>
        </FileUpload.Trigger>
        <FileUpload.List />
      </FileUpload.Root>
    </Field.Root>
  );
}
