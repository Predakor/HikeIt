import type { IconType } from "react-icons";

export type PercentUnit = "%";
export type LengthUnit = "m" | "km";
export type TimeUnit = "s" | "min" | "hrs";

export type SpeedUnit = `${LengthUnit}/${TimeUnit}`;

export type UnitTypes = LengthUnit | TimeUnit | PercentUnit | SpeedUnit;

export type IconUnit = {
  IconType: IconType;
  unit?: UnitTypes;
};

export type StatAddons = {
  IconSource?: IconType;
  unit?: UnitTypes;
  badge?: ReactElement;
  formatt?: (stat) => string | number | null;
};
