import type {
  BaseTrip,
  TripAnalytic,
} from "@/components/AddTripForm/AddTrip/tripTypes";
import type { TripDtoFull } from "@/components/AddTripForm/AddTrip/types";
import type { ReactElement } from "react";
import type { IconType } from "react-icons";
import { CiCalendarDate } from "react-icons/ci";
import {
  FaArrowDown,
  FaArrowTrendDown,
  FaArrowTrendUp,
  FaArrowUp,
} from "react-icons/fa6";
import { GiJourney, GiPeaks } from "react-icons/gi";
import { LuTimer } from "react-icons/lu";
import type { PartialMap } from "../MappingTypes";

type LengthUnit = "m" | "km";
type TimeUnit = "s" | "min" | "hrs";

export type IconUnit = {
  IconType: IconType;
  unit?: LengthUnit | TimeUnit;
};

export type StatAddons = {
  IconSource: IconType;
  unit?: LengthUnit | TimeUnit;
  badge?: ReactElement;
};

export const icons: PartialMap<BaseTrip, StatAddons> = {
  distance: { IconSource: GiJourney, unit: "m" },
  duration: { IconSource: LuTimer, unit: "min" },
  height: { IconSource: FaArrowTrendUp, unit: "m" },
  tripDay: { IconSource: CiCalendarDate },
};

export const analyticIcons: PartialMap<TripAnalytic, StatAddons> = {
  totalDistanceKm: { IconSource: GiJourney, unit: "m" },
  totalAscent: { IconSource: FaArrowTrendUp, unit: "m" },
  totalDescent: { IconSource: FaArrowTrendDown, unit: "m" },
  minElevation: { IconSource: FaArrowDown, unit: "m" },
  maxElevation: { IconSource: FaArrowUp, unit: "m" },
};

export const menuIcons: PartialMap<TripDtoFull, IconType> = {
  reachedPeaks: GiPeaks,
};
