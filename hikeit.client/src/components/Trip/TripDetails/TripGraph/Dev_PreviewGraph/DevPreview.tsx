import { copyToClipboard } from "@/Utils/CopyToClipboard";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { PrimaryButton, SecondaryButton } from "@/components/ui/Buttons";
import { Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { useParams } from "react-router";
import type { ChartData } from "../grap.types";
import useChartPreview from "./useChartPreview";

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
    min: 0.01,
    max: 0.5,
    step: 0.01,
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
  const formHook = useForm<ElevationProfileConfig>();

  const send = useChartPreview(tripId, onSubmit);

  const copyConfig = () => copyToClipboard(formHook.getValues());

  return (
    <form onSubmit={(e) => formHook.handleSubmit(send)(e)}>
      <Stack flexGrow={1} gap={4}>
        <RenderInputs config={elevationDevConfig} formHook={formHook} />

        <PrimaryButton type={"submit"}>Preview</PrimaryButton>

        <SecondaryButton onClick={copyConfig}>Copy Config</SecondaryButton>
      </Stack>
    </form>
  );
}
