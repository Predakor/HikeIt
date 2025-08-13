type RequestState = {
  isPending: boolean;
  isSuccess: boolean;
  isError: boolean;
};

export function isPendingOrNotFetched(requestState: RequestState) {
  return (
    requestState.isPending || (!requestState.isSuccess && !requestState.isError)
  );
}

export function isNotFetched(requestState: RequestState) {
  return !requestState.isSuccess && !requestState.isError;
}
