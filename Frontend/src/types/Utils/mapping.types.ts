import type { IconType } from "react-icons";

export type PartialMap<TObject extends object, TValue> = Partial<Record<keyof TObject, TValue>>;
export type FullMap<TObject extends object, TValue> = Record<keyof TObject, TValue>;

/** Turns `{ a: A; b: B }` into `{ a: Record<number, A>; b: Record<number, B> }`: each key looked up by index. */
export type ByIndex<TValues> = {
  [K in keyof TValues]: Record<number, TValues[K]>;
};

export type PartialIconMap<TObject extends object> = PartialMap<TObject, IconType>;
export type FullIconMap<TObject extends object> = FullMap<TObject, IconType>;
