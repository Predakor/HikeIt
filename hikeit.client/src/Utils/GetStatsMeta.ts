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

export default function GetStatsMeta(name: keyof Stat) {
  const res = statsMeta[name];
  if (!res) {
    console.error(`stat: ${name} not found`);
  }

  return statsMeta[name];
}
