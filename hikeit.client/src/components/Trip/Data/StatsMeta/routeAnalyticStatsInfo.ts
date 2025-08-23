import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import type { StatsMetaList } from "@/types/Utils/stat.types";
import { IconArrowDown, IconArrowUp, IconJourney } from "@/Icons/Icons";
import type { RouteAnalytic } from "@/types/ApiTypes/Analytics";

export const routeAnalyticStatsInfo: StatsMetaList<RouteAnalytic> = {
  totalDistanceMeters: {
    IconSource: IconJourney,
    unit: "m",
    formatt: GenericFormatter,
  },
  totalAscentMeters: {
    IconSource: IconArrowUp,
    unit: "m",
    formatt: GenericFormatter,
  },
  totalDescentMeters: {
    IconSource: IconArrowDown,
    unit: "m",
    formatt: GenericFormatter,
  },
  lowestElevationMeters: {
    IconSource: IconArrowDown,
    unit: "m",
    formatt: GenericFormatter,
  },
  highestElevationMeters: {
    IconSource: IconArrowUp,
    unit: "m",
    formatt: GenericFormatter,
  },
  averageSlopePercent: {
    IconSource: IconArrowUp,
    unit: "%",
    formatt: GenericFormatter,
  },
  averageAscentSlopePercent: {
    IconSource: IconArrowUp,
    unit: "%",
    formatt: GenericFormatter,
  },
  averageDescentSlopePercent: {
    IconSource: IconArrowUp,
    unit: "%",
    formatt: GenericFormatter,
  },
};
