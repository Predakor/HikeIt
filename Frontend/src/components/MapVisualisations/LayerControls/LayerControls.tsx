import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { Stack } from "@chakra-ui/react";
import { memo } from "react";
import { useLayerForm } from "../MapSettings/hooks";
import { layerConfig } from "./useLayerControls";

function LayerControls() {
  const form = useLayerForm();

  return (
    <Stack gap={4} paddingY={3}>
      <RenderInputs config={layerConfig} formHook={form} />
    </Stack>
  );
}

export default memo(LayerControls);
