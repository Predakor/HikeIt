import { Button, Field, FileUpload, type FileUploadFileChangeDetails } from "@chakra-ui/react";
import { HiUpload } from "react-icons/hi";

interface AddFileProps {
  onFileChange: (file: File) => void;
}

export function AddFile({ onFileChange }: AddFileProps) {
  const handleFileChange = (f: FileUploadFileChangeDetails) => {
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
