import { formatDuration } from "@/Utils/Formatters";
import RowStat from "@/components/Stats/RowStat";
import type { StatAddons } from "@/types/Utils/stat.types";
import StatsCard from "../Shared/StatsCard";
import { formatter } from "../Utils/formatter";
import type { Totals } from "../Utils/statTypes";
import {
  IconArrowDown,
  IconArrowUp,
  IconJourney,
  IconTrendDown,
  IconTrendUp,
} from "@/Icons/Icons";

interface Props {
  totals: Totals;
}

export function AverageTrip({ totals }: Props) {
  const {
    totalAscentMeters,
    totalDescentMeters,
    totalDistanceMeters,
    totalDuration,
    totalTrips,
  } = totals;

  const __mockupAscentTime = "07:12:10";
  const __mockupDescentTime = "05:12:10";

  const duration = toMinutes(totalDuration);
  const ascentDuration = toMinutes(__mockupAscentTime);
  const descentDuration = toMinutes(__mockupDescentTime);

  return (
    <StatsCard title="Average Trip">
      <RowStat
        label="Distance"
        value={totalDistanceMeters / totalTrips}
        addons={{ ...distanceAddons, IconSource: IconJourney }}
      />
      <RowStat
        label="Ascent"
        value={totalAscentMeters / totalTrips}
        addons={{ ...distanceAddons, IconSource: IconArrowUp }}
      />
      <RowStat
        label="Descent"
        value={totalDescentMeters / totalTrips}
        addons={{ ...distanceAddons, IconSource: IconArrowDown }}
      />

      <RowStat
        label="Duration"
        value={duration / totalTrips}
        addons={{ ...durationAddons, IconSource: IconJourney }}
      />

      <RowStat
        label={"Ascent Time"}
        value={ascentDuration / totalTrips}
        addons={{ ...durationAddons, IconSource: IconTrendUp }}
      />
      <RowStat
        label={"DescentTime"}
        value={descentDuration / totalTrips}
        addons={{ ...durationAddons, IconSource: IconTrendDown }}
      />
    </StatsCard>
  );
}

const distanceAddons = {
  formatt: formatter.toKm,
  unit: "km",
} satisfies StatAddons<number>;

const durationAddons = {
  formatt: formatter.toHours,
  unit: "h",
} satisfies StatAddons<number>;

const toMinutes = (duration: string) => {
  return Number(formatDuration.toMinutes(duration)) ?? 0;
};
