import GpxArrayBuilder from "@/Utils/Builders/GpxArrayBuilder";
import { Field, FileUpload, Button } from "@chakra-ui/react";
import type { FileChangeDetails } from "node_modules/@chakra-ui/react/dist/types/components/file-upload/namespace";
import { useState } from "react";
import { HiUpload } from "react-icons/hi";
import type { GpxArray } from "../AddTrip/types";
import TripChart from "../TripChart/TripChart";

interface AddFileProps {
  onFileChange: (file: File) => void;
}

export function AddFile({ onFileChange }: AddFileProps) {
  const [chartData, setChartData] = useState<GpxArray | null>(null);

  const handleFileChange = (f: FileChangeDetails) => {
    const file = f.acceptedFiles[0];
    onFileChange(file);
    handleFileMapping(file);
  };

  const handleFileMapping = async (file: File) => {
    const builder = await GpxArrayBuilder.fromFile(file);
    const gpxArray = builder.smoothMedian(5).generateGains().build();

    const stats = builder.getStats();
    console.log({ stats, gpxArray });

    const chartGpxArray = builder.downsample(500).smoothMedian(10).build();

    setChartData(chartGpxArray);
  };

  if (chartData) {
    return <TripChart data={chartData} />;
  }

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
