"use client";
import { Alert } from "@chakra-ui/react";
import type { MutationResult } from "./AddPeakForm";

interface Props {
  requestState: MutationResult;
}

export function AddPeakResult({ requestState }: Props) {
  if (isPendingOrNotFetched(requestState)) {
    return;
  }

  const title = requestState.isSuccess
    ? "Created succesfuly"
    : "Failed to create";

  const description = requestState.isSuccess
    ? "View new peak at location: " + requestState.data?.location
    : requestState.error?.message;

  const status = requestState.isSuccess ? "success" : "error";

  return (
    <Alert.Root status={status}>
      <Alert.Indicator />
      <Alert.Content>
        <Alert.Title>{title}</Alert.Title>
        <Alert.Description>{description}</Alert.Description>
      </Alert.Content>
    </Alert.Root>
  );
}

export function isPendingOrNotFetched(requestState: {
  isPending: Boolean;
  isSuccess: Boolean;
  isError: boolean;
}) {
  return (
    requestState.isPending || (!requestState.isSuccess && !requestState.isError)
  );
}
