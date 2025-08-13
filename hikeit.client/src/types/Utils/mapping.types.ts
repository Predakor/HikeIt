import type { IconType } from "react-icons";

export type PartialMap<TObject extends object, TValue> = Partial<
  Record<keyof TObject, TValue>
>;
export type FullMap<TObject extends object, TValue> = Record<
  keyof TObject,
  TValue
>;

export type PartialIconMap<TObject extends object> = PartialMap<
  TObject,
  IconType
>;
export type FullIconMap<TObject extends object> = FullMap<TObject, IconType>;
