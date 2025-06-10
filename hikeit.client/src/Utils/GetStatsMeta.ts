import {
  icons,
  routeAnalyticStatsInfo,
} from "@/components/Trip/Data/statsInfo";
import type { StatAddons } from "@/types/Utils/StatTypes";

const statsMeta = {
  ...routeAnalyticStatsInfo,
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
