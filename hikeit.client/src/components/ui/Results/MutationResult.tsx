"use client";
import { isPendingOrNotFetched } from "@/Utils/Api/queryHelpers";
import { Alert } from "@chakra-ui/react";
import type { UseMutationResult } from "@tanstack/react-query";

type ResultMessage = {
  title: string;
  description: string;
};

interface Props {
  requestState: UseMutationResult | UseMutationResult<any, any, any, any>;
  succesMesage?: ResultMessage;
  errorMessage?: ResultMessage;
}

export function MutationResult({
  requestState,
  succesMesage = defaultSucces,
  errorMessage = defaultError,
}: Props) {
  if (isPendingOrNotFetched(requestState)) {
    return;
  }

  const status = requestState.isSuccess ? "success" : "error";
  const message = requestState.isSuccess ? succesMesage : errorMessage;

  return (
    <Alert.Root status={status}>
      <Alert.Indicator />
      <Alert.Content>
        <Alert.Title>{message.title}</Alert.Title>
        <Alert.Description>{message.description}</Alert.Description>
      </Alert.Content>
    </Alert.Root>
  );
}

const defaultSucces = {
  title: "Succes",
  description: "Your ressource was created succesfuly",
};
const defaultError = {
  title: "Fail",
  description: "Something went wrong while processing your request",
};
