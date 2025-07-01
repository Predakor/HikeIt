import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { IconArrowDown, IconArrowUp, IconJourney } from "@/assets/Icons";
import type { StatsMetaList } from "@/types/Utils/StatTypes";
import type { RouteAnalytic } from "../../Types/TripAnalyticsTypes";

export const routeAnalyticStatsInfo: StatsMetaList<RouteAnalytic> = {
  totalDistanceKm: {
    IconSource: IconJourney,
    unit: "m",
    formatt: GenericFormatter,
  },
  totalAscent: {
    IconSource: IconArrowUp,
    unit: "m",
    formatt: GenericFormatter,
  },
  totalDescent: {
    IconSource: IconArrowDown,
    unit: "m",
    formatt: GenericFormatter,
  },
  lowestElevation: {
    IconSource: IconArrowDown,
    unit: "m",
    formatt: GenericFormatter,
  },
  highestElevation: {
    IconSource: IconArrowUp,
    unit: "m",
    formatt: GenericFormatter,
  },
  averageSlope: {
    IconSource: IconArrowUp,
    unit: "%",
    formatt: GenericFormatter,
  },
  averageAscentSlope: {
    IconSource: IconArrowUp,
    unit: "%",
    formatt: GenericFormatter,
  },
  averageDescentSlope: {
    IconSource: IconArrowUp,
    unit: "%",
    formatt: GenericFormatter,
  },
};
