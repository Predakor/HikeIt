import {
  Box,
  Field,
  FileUpload,
  Icon,
  VisuallyHidden,
  type FileUploadRootProps,
} from "@chakra-ui/react";
import { LuUpload } from "react-icons/lu";

interface AddFileProps extends Omit<FileUploadRootProps, "onFileChange"> {
  onFileChange: (file: File) => void;
  allowedFiles: string[];
  label: string;
}

function DropFile({ onFileChange, allowedFiles, label }: AddFileProps) {
  return (
    <>
      <VisuallyHidden>
        <Field.Label>{label}</Field.Label>
      </VisuallyHidden>
      <FileUpload.Root
        accept={allowedFiles}
        onFileChange={(f) => onFileChange(f.acceptedFiles[0])}
        alignItems="stretch"
      >
        <FileUpload.HiddenInput />
        <FileUpload.Dropzone>
          <Icon size="md" color="fg.muted">
            <LuUpload />
          </Icon>
          <FileUpload.DropzoneContent>
            <Box>Drag and drop files here</Box>
            <Box color="fg.muted">{allowedFiles.join(" ")}</Box>
          </FileUpload.DropzoneContent>
        </FileUpload.Dropzone>
        <FileUpload.List />
      </FileUpload.Root>
    </>
  );
}
export default DropFile;
