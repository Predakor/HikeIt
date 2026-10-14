import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { Stack } from "@chakra-ui/react";
import { memo } from "react";
import type { UseFormReturn } from "react-hook-form";
import { layerConfig, type LayerOptions } from "./useLayerControls";

function LayerControls({ form }: { form: UseFormReturn<LayerOptions> }) {
  return (
    <Stack gap={4} paddingY={3}>
      <RenderInputs config={layerConfig} formHook={form} />
    </Stack>
  );
}

export default memo(LayerControls);
