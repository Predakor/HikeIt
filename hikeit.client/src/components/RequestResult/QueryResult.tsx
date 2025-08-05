import { Alert, LinkBox } from "@chakra-ui/react";
import type { UseMutationResult } from "@tanstack/react-query";
import { Link } from "react-router";

interface Props {
  mutation: UseMutationResult<
    {
      location: string;
    },
    Error,
    object,
    unknown
  >;
}

function QueryResult({ mutation }: Props) {
  const { isSuccess, isError, isPending, data, error } = mutation;

  const alertTitle = isSuccess ? "Created succesfully" : "Failed to create";

  if (isPending) {
    <Alert.Root status={"neutral"}>
      <Alert.Indicator />
      <Alert.Content>
        <Alert.Title>Generating your trip</Alert.Title>
        <Alert.Description>"Hold still"</Alert.Description>
      </Alert.Content>
    </Alert.Root>;
  }

  return (
    (isSuccess || isError) && (
      <Alert.Root status={isError ? "error" : "success"}>
        <Alert.Indicator />
        <Alert.Content>
          <Alert.Title>{alertTitle}</Alert.Title>
          <Alert.Description>
            {data ? (
              <LinkBox asChild>
                <Link to={data.location}>click me to view created trip</Link>
              </LinkBox>
            ) : (
              <>{error.message ?? "unknow reason"}</>
            )}
          </Alert.Description>
        </Alert.Content>
      </Alert.Root>
    )
  );
}
export default QueryResult;
