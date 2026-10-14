/** Picks the member of a `{ type }` union that has the given type. */
export type ActionOf<TAction extends { type: string }, TType extends TAction["type"]> = Extract<
  TAction,
  { type: TType }
>;

/** Payload that sets `value` at `index` in the record stored under key K (the action side of `ByIndex`). */
export type SetAtIndex<TValues, K extends keyof TValues> = { index: number; value: TValues[K] };

/** One `{ type: K; index; value }` action per key of TValues, as a union. */
export type SetAtIndexActions<TValues> = {
  [K in keyof TValues]: { type: Extract<K, string> } & SetAtIndex<TValues, K>;
}[keyof TValues];

/** One handler per action type, so a missing handler is a compile error. */
export type ReducerHandlers<TState, TAction extends { type: string }> = {
  [TType in TAction["type"]]: (state: TState, action: ActionOf<TAction, TType>) => TState;
};
