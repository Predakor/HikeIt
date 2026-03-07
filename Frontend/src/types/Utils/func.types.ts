export type Action<TIn> = (item: TIn) => void;
export type Func<TIn, TOut> = (item: TIn) => TOut;
export type Predicate<TIn> = (item: TIn) => boolean;
