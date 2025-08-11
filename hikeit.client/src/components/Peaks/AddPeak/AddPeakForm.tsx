"use client";
import SubTitle from "@/components/Titles/SubTitle";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import { Button, Separator, Stack } from "@chakra-ui/react";
import type { UseMutationResult } from "@tanstack/react-query";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { AddPeakResult, isPendingOrNotFetched } from "./AddPeakResult";
import { FormHelpers } from "./FormHelpers";
import { addPeakFormConfig, type AddPeakConfig } from "./addPeakFormConfig";

export type MutationResult = UseMutationResult<
  {
    location: string;
  },
  Error,
  AddPeakConfig,
  unknown
>;

interface Props {
  onSubmit: (data: AddPeakConfig) => void;
  requestState: MutationResult;
}

export default function AddPeakForm({ requestState, onSubmit }: Props) {
  const formHook = useForm<AddPeakConfig>();

  useEffect(() => {
    if (isPendingOrNotFetched(requestState)) {
      return;
    }

    if (requestState.isSuccess) {
      queueMicrotask(() => {
        formHook.reset();
      });
    }
  }, [requestState.isSuccess]);

  return (
    <Stack gapY={8}>
      <AddPeakResult requestState={requestState} />

      <Stack>
        <form onSubmit={formHook.handleSubmit(onSubmit)}>
          <Stack gapY={4}>
            <RenderInputs
              displayOptions={{ size: "xl" }}
              config={addPeakFormConfig}
              formHook={formHook}
            />

            <Button
              loading={requestState.isPending}
              loadingText={"Uploading"}
              colorPalette={"blue"}
              size={"xl"}
              type="submit"
            >
              Upload
            </Button>
          </Stack>
        </form>
      </Stack>
      <Separator />
      <Stack gapY={4}>
        <SubTitle title="Quick fills" />
        <FormHelpers setForm={formHook.setValue} />
      </Stack>
    </Stack>
  );
}
