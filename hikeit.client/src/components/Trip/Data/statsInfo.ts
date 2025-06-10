import type { BaseTrip, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { IconBaseProps, IconType } from "react-icons";
import { CiCalendarDate } from "react-icons/ci";
import {
  FaArrowDown,
  FaArrowTrendDown,
  FaArrowTrendUp,
  FaArrowUp,
} from "react-icons/fa6";
import { GiJourney, GiPeaks } from "react-icons/gi";
import type { FullMap, PartialMap } from "../../../types/Utils/MappingTypes";
import type { StatAddons } from "@/types/Utils/StatTypes";
import type { RouteAnalytic, TimeAnalytics } from "../Types/TripAnalyticsTypes";
import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import type { ReactNode } from "react";

export const icons: PartialMap<BaseTrip, StatAddons> = {
  // distance: { IconSource: GiJourney, unit: "m" },
  // duration: { IconSource: LuTimer, unit: "min" },
  // height: { IconSource: FaArrowTrendUp, unit: "m" },
  tripDay: { IconSource: CiCalendarDate },
};

function routeFormatter(stat: any): string | number {
  return GenericFormatter(stat);
}
function durationFormatter(stat: any): string | number | null {
  const [hours, minutes, seconds] = stat.split(":").map(Number);
  const date = new Date();
  date.setHours(hours, minutes, seconds, 0);
  return hours * 60 + date.getMinutes();
  return stat;
}

export const routeAnalyticStatsInfo: PartialMap<RouteAnalytic, StatAddons> = {
  totalDistanceKm: {
    IconSource: GiJourney,
    unit: "m",
    formatt: routeFormatter,
  },
  totalAscent: {
    IconSource: FaArrowTrendUp,
    unit: "m",
    formatt: routeFormatter,
  },
  totalDescent: {
    IconSource: FaArrowTrendDown,
    unit: "m",
    formatt: routeFormatter,
  },
  lowestElevation: {
    IconSource: FaArrowDown,
    unit: "m",
    formatt: routeFormatter,
  },
  highestElevation: {
    IconSource: FaArrowUp,
    unit: "m",
    formatt: routeFormatter,
  },
  averageSlope: { IconSource: FaArrowUp, unit: "%", formatt: routeFormatter },
  averageAscentSlope: {
    IconSource: FaArrowUp,
    unit: "%",
    formatt: routeFormatter,
  },
  averageDescentSlope: {
    IconSource: FaArrowUp,
    unit: "%",
    formatt: routeFormatter,
  },
};

export const timeAnalyticsStatsInfo: PartialMap<TimeAnalytics, StatAddons> = {
  duration: {
    IconSource: FaArrowUp,
    unit: "min",
    formatt: GenericFormatter,
  },
  startTime: {
    IconSource: FaArrowUp,
    formatt: GenericFormatter,
  },
  endTime: {
    IconSource: FaArrowUp,
    formatt: GenericFormatter,
  },
  activeTime: {
    IconSource: FaArrowUp,
    unit: "min",
    formatt: durationFormatter,
  },
  idleTime: {
    IconSource: FaArrowUp,
    unit: "min",
    formatt: durationFormatter,
  },
  ascentTime: {
    IconSource: FaArrowUp,
    unit: "min",
    formatt: durationFormatter,
  },
  descentTime: {
    IconSource: FaArrowUp,
    unit: "min",
    formatt: durationFormatter,
  },
  averageSpeedKph: {
    IconSource: FaArrowUp,
    unit: "km/hrs",
    formatt: undefined,
  },
  averageAscentKph: {
    IconSource: FaArrowUp,
    unit: "km/hrs",
    formatt: undefined,
  },
  averageDescentKph: {
    IconSource: FaArrowDown,
    unit: "km/hrs",
    formatt: undefined,
  },
};

export const menuIcons: PartialMap<TripDtoFull, IconType> = {
  reachedPeaks: GiPeaks,
};
