import type { IconType } from "react-icons";
import type { ReactElement } from "react";
import type { PartialMap } from "@/types/Utils/mapping.types";

export type PercentUnit = "%";
export type LengthUnit = "m" | "km";
export type TimeUnit = "s" | "min" | "h";
export type SpeedUnit = `${LengthUnit}/${TimeUnit}`;
export type UnitTypes = LengthUnit | TimeUnit | PercentUnit | SpeedUnit;

export type IconUnit = {
  IconType: IconType;
  unit?: UnitTypes;
};

export type StatAddons<T> = {
  IconSource?: IconType;
  unit?: UnitTypes;
  badge?: ReactElement;
  formatt?: (stat: T) => string | number | null;
};

export type StatsMetaList<T extends object> = PartialMap<
  T,
  StatAddons<string | number>
>;
