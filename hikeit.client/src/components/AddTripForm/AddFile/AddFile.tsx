import { Button, Field, FileUpload } from "@chakra-ui/react";
import type { FileChangeDetails } from "node_modules/@chakra-ui/react/dist/types/components/file-upload/namespace";
import { HiUpload } from "react-icons/hi";

interface AddFileProps {
  onFileChange: (file: File) => void;
}

export function AddFile({ onFileChange }: AddFileProps) {
  const handleFileChange = (f: FileChangeDetails) => {
    const file = f.acceptedFiles[0];
    onFileChange(file);
  };

  return (
    <Field.Root>
      <Field.Label>Gpx file</Field.Label>
      <FileUpload.Root accept={".gpx"} onFileChange={handleFileChange}>
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
