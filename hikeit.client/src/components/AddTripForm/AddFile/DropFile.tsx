import { Box, Field, FileUpload, Icon } from "@chakra-ui/react";
import type { FileChangeDetails } from "node_modules/@chakra-ui/react/dist/types/components/file-upload/namespace";
import { LuUpload } from "react-icons/lu";

interface AddFileProps {
  onFileChange: (file: File) => void;
  allowedFiles: string[];
}

function DropFile({ onFileChange, allowedFiles }: AddFileProps) {
  const handleFileChange = (f: FileChangeDetails) => {
    const file = f.acceptedFiles[0];
    onFileChange(file);
  };

  return (
    <Field.Root>
      <Field.Label>Gpx file</Field.Label>
      <FileUpload.Root
        accept={allowedFiles}
        onFileChange={handleFileChange}
        maxW="xl"
        alignItems="stretch"
      >
        <FileUpload.HiddenInput />
        <FileUpload.Dropzone>
          <Icon size="md" color="fg.muted">
            <LuUpload />
          </Icon>
          <FileUpload.DropzoneContent>
            <Box>Drag and drop files here</Box>
            <Box color="fg.muted">
              {allowedFiles.map((allowedFile) => allowedFile)}
            </Box>
          </FileUpload.DropzoneContent>
        </FileUpload.Dropzone>
        <FileUpload.List />
      </FileUpload.Root>
    </Field.Root>
  );
}
export default DropFile;
