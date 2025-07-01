import { toMinutes } from "@/Utils/Formatters/durationFormatter";
import {
  GenericFormatter,
  NumberFormatter,
} from "@/Utils/Formatters/valueFormatter";
import {
  IconClock,
  IconHiking,
  IconMountain,
  IconMountainSun,
  IconPause,
  IconPlay,
  IconSpeed,
  IconStop,
  IconTrendDown,
  IconTrendUp,
} from "@/assets/Icons";
import type { StatsMetaList } from "@/types/Utils/StatTypes";
import type { TimeAnalytic } from "../../Types/TripAnalyticsTypes";

export const timeAnalyticsStatsInfo: StatsMetaList<TimeAnalytic> = {
  duration: {
    IconSource: IconClock,
    unit: "min",
    formatt: GenericFormatter,
  },
  startTime: {
    IconSource: IconPlay,
    formatt: GenericFormatter,
  },
  endTime: {
    IconSource: IconStop,
    formatt: GenericFormatter,
  },
  activeTime: {
    IconSource: IconHiking,
    unit: "min",
    formatt: toMinutes,
  },
  idleTime: {
    IconSource: IconPause,
    unit: "min",
    formatt: toMinutes,
  },
  ascentTime: {
    IconSource: IconMountainSun,
    unit: "min",
    formatt: toMinutes,
  },
  descentTime: {
    IconSource: IconMountain,
    unit: "min",
    formatt: toMinutes,
  },
  averageSpeedKph: {
    IconSource: IconSpeed,
    unit: "km/hrs",
    formatt: (num) => NumberFormatter(num, 2),
  },
  averageAscentKph: {
    IconSource: IconTrendUp,
    unit: "km/hrs",
    formatt: (num) => NumberFormatter(num, 2),
  },
  averageDescentKph: {
    IconSource: IconTrendDown,
    unit: "km/hrs",
    formatt: (num) => NumberFormatter(num, 2),
  },
};
