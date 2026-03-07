import {
  IconArrowDown,
  IconArrowUp,
  IconJourney,
  IconTrendDown,
  IconTrendUp,
} from "@/Icons/Icons";
import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";
import { RowStat } from "@/components/Stats";
import StatsCard from "../Shared/StatsCard";
import { formatter } from "../Utils/formatter";
import type { Totals } from "../Utils/statTypes";

export function TotalsStats({ stats }: { stats: Totals }) {
  const totalActiveDuration = TimeSpan.From(stats.totalDuration);
  const totalAscentDuration = TimeSpan.From(stats.totalDuration);
  const totalDescentDuration = TimeSpan.From(stats.totalDuration);

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

      <RowStat
        label="Total Hiking Time"
        value={totalActiveDuration}
        addons={{ formatt: (t) => t.toString(), IconSource: IconJourney }}
      />
      <RowStat
        label="Climb Time"
        value={totalAscentDuration}
        addons={{ formatt: (t) => t.toString(), IconSource: IconTrendUp }}
      />
      <RowStat
        label="Descent Time"
        value={totalDescentDuration}
        addons={{ formatt: (t) => t.toString(), IconSource: IconTrendDown }}
      />
    </StatsCard>
  );
}
