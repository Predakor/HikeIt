import {
  GenericFormatter,
  NumberFormatter,
} from "@/Utils/Formatters/valueFormatter";
import type { BaseTrip, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { StatAddons } from "@/types/Utils/StatTypes";
import type { IconType } from "react-icons";
import { CiCalendarDate } from "react-icons/ci";
import { FaPauseCircle } from "react-icons/fa";
import {
  FaArrowDown,
  FaArrowTrendDown,
  FaArrowTrendUp,
  FaArrowUp,
  FaMountain,
  FaMountainSun,
  FaPersonHiking,
  FaPlay,
  FaRegClock,
  FaStop,
} from "react-icons/fa6";
import { GiJourney, GiPeaks } from "react-icons/gi";
import { SiSpeedtest } from "react-icons/si";
import type { PartialMap } from "../../../types/Utils/MappingTypes";
import type { RouteAnalytic, TimeAnalytic } from "../Types/TripAnalyticsTypes";

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

export const timeAnalyticsStatsInfo: PartialMap<TimeAnalytic, StatAddons> = {
  duration: {
    IconSource: FaRegClock,
    unit: "min",
    formatt: GenericFormatter,
  },
  startTime: {
    IconSource: FaPlay,
    formatt: GenericFormatter,
  },
  endTime: {
    IconSource: FaStop,
    formatt: GenericFormatter,
  },
  activeTime: {
    IconSource: FaPersonHiking,
    unit: "min",
    formatt: durationFormatter,
  },
  idleTime: {
    IconSource: FaPauseCircle,
    unit: "min",
    formatt: durationFormatter,
  },
  ascentTime: {
    IconSource: FaMountainSun,
    unit: "min",
    formatt: durationFormatter,
  },
  descentTime: {
    IconSource: FaMountain,
    unit: "min",
    formatt: durationFormatter,
  },
  averageSpeedKph: {
    IconSource: SiSpeedtest,
    unit: "km/hrs",
    formatt: (num) => NumberFormatter(num, 2),
  },
  averageAscentKph: {
    IconSource: FaArrowTrendUp,
    unit: "km/hrs",
    formatt: (num) => NumberFormatter(num, 2),
  },
  averageDescentKph: {
    IconSource: FaArrowTrendDown,
    unit: "km/hrs",
    formatt: (num) => NumberFormatter(num, 2),
  },
};

export const menuIcons: PartialMap<TripDtoFull, IconType> = {
  reachedPeaks: GiPeaks,
};
