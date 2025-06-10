import type { BaseTrip, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { IconType } from "react-icons";
import { CiCalendarDate } from "react-icons/ci";
import {
  FaArrowDown,
  FaArrowTrendDown,
  FaArrowTrendUp,
  FaArrowUp,
} from "react-icons/fa6";
import { GiJourney, GiPeaks } from "react-icons/gi";
import type { PartialMap } from "../../../types/Utils/MappingTypes";
import type { StatAddons } from "@/types/Utils/StatTypes";
import type { RouteAnalytic } from "../Types/TripAnalyticsTypes";
import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";

export const icons: PartialMap<BaseTrip, StatAddons> = {
  // distance: { IconSource: GiJourney, unit: "m" },
  // duration: { IconSource: LuTimer, unit: "min" },
  // height: { IconSource: FaArrowTrendUp, unit: "m" },
  tripDay: { IconSource: CiCalendarDate },
};

function routeFormatter(stat: any): string | number {
  return GenericFormatter(stat);
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

export const menuIcons: PartialMap<TripDtoFull, IconType> = {
  reachedPeaks: GiPeaks,
};
