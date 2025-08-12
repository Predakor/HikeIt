type RequestState = {
  isPending: Boolean;
  isSuccess: Boolean;
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
