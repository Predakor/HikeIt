import type { FetchResponse } from "@/hooks/useFetch";
import type { FunctionComponent, ReactNode } from "react";

interface Props<T> {
  LoadingComponent?: FunctionComponent;
  ErrorComponent?: FunctionComponent<{ error: string }>;
  NoDataComponent?: FunctionComponent;
  children: (data: T) => ReactNode; // render prop for data
  request: FetchResponse<T>;
}

const DefaultLoading = () => <div>Loading...</div>;
const DefaultError = () => <div>Something went wrong.</div>;
const DefaultNoData = () => <div>No data available.</div>;

function FetchWrapper<T>({
  children,
  request,
  LoadingComponent = DefaultLoading,
  ErrorComponent = DefaultError,
  NoDataComponent = DefaultNoData,
}: Props<T>) {
  const { data, error, loading } = request;

  if (loading) {
    return <LoadingComponent />;
  }

  if (error) {
    return <ErrorComponent error={error} />;
  }

  if (!data) {
    return <NoDataComponent />;
  }

  return <>{children(data)}</>;
}

export default FetchWrapper;
