import { icons } from "@/components/Trip/Data/StatsMeta/statsInfo";
import { timeAnalyticsStatsInfo } from "@/components/Trip/Data/StatsMeta/timeAnalyticsStatsInfo";
import { routeAnalyticStatsInfo } from "@/components/Trip/Data/StatsMeta/routeAnalyticStatsInfo";
import type { StatAddons } from "@/types/Utils/stat.types";

const statsMeta = {
  ...routeAnalyticStatsInfo,
  ...timeAnalyticsStatsInfo,
  ...icons,
};

export type Stat = Record<keyof typeof statsMeta, StatAddons>;

export default function GetStatsMeta(name: keyof Stat | string) {
  const res = statsMeta[name as keyof Stat];
  if (!res) {
    console.error(`stat: ${name} not found`);
  }

  return statsMeta[name as keyof Stat];
}
