import {
  calculateStats,
  downsampleToMaxSize,
  generateGains,
  smoothMedian,
} from "@/Utils/arrayUtils";
import { Button, Field, FileUpload } from "@chakra-ui/react";
import type { FileChangeDetails } from "node_modules/@chakra-ui/react/dist/types/components/file-upload/namespace";
import { useState } from "react";
import { HiUpload } from "react-icons/hi";
import type { GpxArray } from "../AddTrip/types";
import TripChart from "../TripChart/TripChart";

interface AddFileProps {
  onFileChange: (file: File) => void;
}

export function AddFile({ onFileChange }: AddFileProps) {
  const handleFileChange = (f: FileChangeDetails) => {
    const file = f.acceptedFiles[0];
    onFileChange(file);
    handleFileMapping(file);
  };

  const [chartData, setChartData] = useState<GpxArray | null>(null);

  const handleFileMapping = async (file: File) => {
    let gpxArray = await mapToGpxArray(file); //map from file
    gpxArray = smoothMedian(gpxArray, 5); //smooth out gps error
    gpxArray = generateGains(gpxArray); // calculate point to point gains

    const stats = calculateStats(gpxArray);
    console.log({ stats, gpxArray });

    let chartGpxArray = downsampleToMaxSize(gpxArray, 500);
    chartGpxArray = smoothMedian(gpxArray, 10);
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

async function mapToGpxArray(file: File): Promise<GpxArray> {
  const text = await file.text();
  const parser = new DOMParser();
  const xml = parser.parseFromString(text, "application/xml");

  return Array.from(xml.getElementsByTagName("trkpt")).map((pt) => ({
    lat: parseFloat(pt.getAttribute("lat") || "0"),
    lon: parseFloat(pt.getAttribute("lon") || "0"),
    ele: parseFloat(pt.getElementsByTagName("ele")[0]?.textContent || "0"),
    time: pt.getElementsByTagName("time")[0]?.textContent || "",
  }));
}
