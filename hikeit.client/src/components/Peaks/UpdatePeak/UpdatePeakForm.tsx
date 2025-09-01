import { diffObjects } from "@/Utils/objectHelpers";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { PeakWithLocation } from "@/types/Api/peak.types";
import { Button, Stack } from "@chakra-ui/react";
import type { UseMutationResult } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import {
  addPeakFormConfig,
  type AddPeakConfig,
} from "../AddPeak/addPeakFormConfig";

interface Props {
  onSubmit: (data: Partial<AddPeakConfig>) => void;
  requestState: UseMutationResult;
  peak?: PeakWithLocation;
}

export default function UpdatePeakForm(props: Props) {
  const { peak, onSubmit, requestState } = props;
  const formHook = useForm<AddPeakConfig>({
    defaultValues: peak,
  });

  const handleFormSubmit = formHook.handleSubmit((updated) => {
    if (!peak) {
      return;
    }

    const changes = diffObjects(peak, updated);

    const hasNoChanges = Object.entries(changes).length === 0;
    if (hasNoChanges) {
      alert("no changes detected");
      return;
    }

    onSubmit(changes);
  });

  return (
    <Stack>
      <form onSubmit={handleFormSubmit}>
        <Stack gapY={4}>
          <RenderInputs
            displayOptions={{ size: "xl" }}
            config={addPeakFormConfig}
            formHook={formHook}
          />

          <Button
            loading={requestState.isPending}
            loadingText={"Updating"}
            colorPalette={"blue"}
            size={"xl"}
            type="submit"
          >
            Update
          </Button>
        </Stack>
      </form>
    </Stack>
  );
}
