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

export type LengthUnit = "m" | "km";
export type TimeUnit = "s" | "min" | "hrs";

export type IconUnit = {
  IconType: IconType;
  unit?: LengthUnit | TimeUnit;
};

export type StatAddons = {
  IconSource: IconType;
  unit?: LengthUnit | TimeUnit;
  badge?: ReactElement;
};
