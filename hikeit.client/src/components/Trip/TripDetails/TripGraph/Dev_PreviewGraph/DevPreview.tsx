import apiClient from "@/Utils/Api/ApiClient";
import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { AllObjectEntriesToArray } from "@/Utils/ObjectToArray";
import { Button, Field, Input, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import type { ChartData } from "../TripGraph";

type ElevationProfileConfig = {
  MaxElevationSpike: number;
  EmaSmoothingAlpha: number;
  MedianFilterWindowSize: number;
  RoundingDecimalsCount: number;
  DownsamplingFactor: number;
  fileId: string;
};

const elevationDevConfig = {
  MaxElevationSpike: {
    type: "number",
  },
  EmaSmoothingAlpha: {
    type: "range",
  },
  MedianFilterWindowSize: {
    type: "number",
  },
  RoundingDecimalsCount: {
    type: "number",
  },
  DownsamplingFactor: {
    type: "number",
  },
};

interface Props {
  onSubmit: (data: ChartData) => void;
}

export function DevConfig({ onSubmit }: Props) {
  const { register, handleSubmit, getValues } =
    useForm<ElevationProfileConfig>();

  const uploadHandler = handleSubmit(async (data) => {
    const id = data.fileId;

    const request = await apiClient<ChartData>(
      `trips/Analytics/elevations/${id}/preview`,
      {
        method: "POST",
        body: JSON.stringify({
          MaxElevationSpike: data.MaxElevationSpike || null,
          EmaSmoothingAlpha: data.EmaSmoothingAlpha / 100 || null,
          MedianFilterWindowSize: data.MedianFilterWindowSize || null,
          RoundingDecimalsCount: data.RoundingDecimalsCount || null,
          DownsamplingFactor: data.DownsamplingFactor || null,
        }),
      }
    );
    console.log(data.EmaSmoothingAlpha / 100);

    if (request) {
      onSubmit(request);
    }
  });

  const handleCopySettings = () => {
    const values = getValues();

    const { fileId, ...config } = values;

    const transformed = {
      ...config,
      EmaSmoothingAlpha: config.EmaSmoothingAlpha
        ? config.EmaSmoothingAlpha / 100
        : null,
    };

    const textToCopy = JSON.stringify(transformed, null, 2);

    navigator.clipboard
      .writeText(textToCopy)
      .then(() => {
        console.log("Settings copied to clipboard!");
      })
      .catch((err) => {
        console.error("Failed to copy settings:", err);
      });
  };

  const x = AllObjectEntriesToArray(elevationDevConfig);
  return (
    <form onSubmit={uploadHandler}>
      <Stack flexGrow={1}>
        {x.map(([key, value]) => (
          <Field.Root key={key} w={40}>
            <Field.Label>{KeyToLabelFormatter(key)}</Field.Label>
            <Input type={value.type} {...register(key)} />
          </Field.Root>
        ))}
        <Button type={"submit"}>Preview</Button>
        <Field.Root>
          <Field.Label>FileID</Field.Label>
          <Input {...register("fileId")} />
        </Field.Root>
        <Button onClick={handleCopySettings}>Copyy</Button>
      </Stack>
    </form>
  );
}
