import apiClient from "@/Utils/Api/ApiClient";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { Button, Field, Input, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import type { ChartData } from "../_graph_types";
import { copyToClipboard } from "@/Utils/CopyToClipboard";

type ElevationProfileConfig = {
  MaxElevationSpike: number;
  EmaSmoothingAlpha: number;
  MedianFilterWindowSize: number;
  RoundingDecimalsCount: number;
  DownsamplingFactor: number;
  fileId: string;
};

const elevationDevConfig: InputsConfig = [
  { key: "MaxElevationSpike", label: "", type: "range", min: 1, max: 10 },
  {
    key: "EmaSmoothingAlpha",
    label: "",
    type: "range",
    min: 1,
    max: 50,
    formatValue: (value) => value / 100,
  },
  { key: "MedianFilterWindowSize", label: "", type: "range", min: 3, max: 10 },
  { key: "RoundingDecimalsCount", label: "", type: "range", min: 0, max: 10 },
  { key: "DownsamplingFactor", label: "", type: "range", min: 1, max: 10 },
];

interface Props {
  onSubmit: (data: ChartData) => void;
}

export function DevConfig({ onSubmit }: Props) {
  const { control, register, handleSubmit, getValues } =
    useForm<ElevationProfileConfig>();

  const uploadHandler = handleSubmit(async (data) => {
    const id = data.fileId;

    const request = await apiClient<ChartData>(
      `trips/Analytics/elevations/${id}/preview`,
      {
        method: "POST",
        body: JSON.stringify({
          MaxElevationSpike: data.MaxElevationSpike || null,
          EmaSmoothingAlpha: data.EmaSmoothingAlpha || null,
          MedianFilterWindowSize: data.MedianFilterWindowSize || null,
          RoundingDecimalsCount: data.RoundingDecimalsCount || null,
          DownsamplingFactor: data.DownsamplingFactor || null,
        }),
      }
    );

    if (request) {
      onSubmit(request);
    }
  });

  return (
    <form onSubmit={uploadHandler}>
      <Stack flexGrow={1}>
        <RenderInputs
          config={elevationDevConfig}
          register={register}
          control={control}
        />

        <Button type={"submit"}>Preview</Button>
        <Field.Root>
          <Field.Label>FileID</Field.Label>
          <Input {...register("fileId")} />
        </Field.Root>
        <Button onClick={() => copyToClipboard(getValues())}>Copyy</Button>
      </Stack>
    </form>
  );
}
