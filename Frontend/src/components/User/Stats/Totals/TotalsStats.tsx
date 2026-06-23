import {
  IconArrowDown,
  IconArrowUp,
  IconClock,
  IconJourney,
  IconTrendDown,
  IconTrendUp,
} from "@/Icons/Icons";
import { RowStat, TimeRowStat } from "@/components/Stats";
import StatsCard from "../Shared/StatsCard";
import { formatter } from "../Utils/formatter";
import type { Totals } from "../Utils/statTypes";

export function TotalsStats({ stats }: { stats: Totals }) {
  const distanceAddons = {
    unit: "km",
    formatt: formatter.toKm,
  } as const;

  return (
    <StatsCard title="Totals">
      <RowStat
        label="Total Distance"
        value={stats.totalDistanceMeters}
        addons={{
          ...distanceAddons,
          IconSource: IconJourney,
        }}
      />
      <RowStat
        label="Total Ascent"
        value={stats.totalAscentMeters}
        addons={{
          ...distanceAddons,
          IconSource: IconArrowUp,
        }}
      />
      <RowStat
        label="Total Descent"
        value={stats.totalDescentMeters}
        addons={{
          ...distanceAddons,
          IconSource: IconArrowDown,
        }}
      />

      <TimeRowStat
        label="Total Hiking Time"
        value={stats.totalDuration}
        addons={{ IconSource: IconClock }}
      />
      <TimeRowStat
        label="Climb Time"
        value={stats.totalClimbDuration}
        addons={{ IconSource: IconTrendUp }}
      />
      <TimeRowStat
        label="Descent Time"
        value={stats.totalDescentDuration}
        addons={{ IconSource: IconTrendDown }}
      />
    </StatsCard>
  );
}
