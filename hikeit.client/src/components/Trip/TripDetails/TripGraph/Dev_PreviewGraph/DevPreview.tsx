import apiClient from "@/Utils/Api/ApiClient";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { Button, Field, Input, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import type { ChartData } from "../_graph_types";
import { copyToClipboard } from "@/Utils/CopyToClipboard";
import useChartPreview from "./useChartPreview";
import { useParams } from "react-router";

export type ElevationProfileConfig = {
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
  const { tripId } = useParams();
  const { control, register, handleSubmit, getValues } =
    useForm<ElevationProfileConfig>();

  const send = useChartPreview(tripId, onSubmit);

  const copyConfig = () => copyToClipboard(getValues());

  return (
    <form onSubmit={handleSubmit(send)}>
      <Stack flexGrow={1}>
        <RenderInputs
          config={elevationDevConfig}
          register={register}
          control={control}
        />

        <Button colorPalette={"blue"} type={"submit"}>
          Preview
        </Button>

        <Button onClick={copyConfig} colorPalette={"green"}>
          Copy Config
        </Button>
      </Stack>
    </form>
  );
}
