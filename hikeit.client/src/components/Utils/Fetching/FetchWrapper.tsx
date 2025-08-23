import { Skeleton } from "@chakra-ui/react";
import { type UseQueryResult } from "@tanstack/react-query";
import type { FunctionComponent, ReactNode } from "react";

interface Props<T> {
  LoadingComponent?: FunctionComponent;
  ErrorComponent?: FunctionComponent<{ error: Error }>;
  NoDataComponent?: FunctionComponent;
  Component?: FunctionComponent<{ data: T }>;
  children?: (data: T) => ReactNode; // render prop for data
  request: UseQueryResult<T>;
}

const DefaultLoading = () => <Skeleton width={"full"} />;
const DefaultError = () => <div>Something went wrong.</div>;
const DefaultNoData = () => <div>No data available.</div>;

function FetchWrapper<T>({
  children,
  request,
  Component,
  LoadingComponent = DefaultLoading,
  ErrorComponent = DefaultError,
  NoDataComponent = DefaultNoData,
}: Props<T>) {
  const { data, error, isLoading } = request;

  if (isLoading) {
    return <LoadingComponent />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  if (!data || (Array.isArray(data) && data.length === 0)) {
    return <NoDataComponent />;
  }

  if (Component) {
    return <Component data={data} />;
  }

  return <>{children!(data)}</>;
}

export default FetchWrapper;
