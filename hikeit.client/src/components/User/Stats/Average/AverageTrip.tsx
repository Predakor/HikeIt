import { formatDuration } from "@/Utils/Formatters";
import RowStat from "@/components/Stats/RowStat";
import type { StatAddons } from "@/types/Utils/stat.types";
import type { Totals, Locations } from "../Utils/statTypes";
import { formatter } from "../Utils/formatter";
import StatsCard from "../StatsCard";

interface Props {
  totals: Totals;
  locations: Locations;
}

export function AverageTrip({ totals, locations }: Props) {
  const {
    totalAscentMeters,
    totalDescentMeters,
    totalDistanceMeters,
    totalDuration,
    totalPeaks,
    totalTrips,
  } = totals;

  const { regionsVisited } = locations;

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
        addons={distanceAddons}
      />
      <RowStat
        label="Ascent"
        value={totalAscentMeters / totalTrips}
        addons={distanceAddons}
      />
      <RowStat
        label="Descent"
        value={totalDescentMeters / totalTrips}
        addons={distanceAddons}
      />

      <RowStat
        label="Duration"
        value={duration / totalTrips}
        addons={durationAddons}
      />

      <RowStat
        label={"Ascent Time"}
        value={ascentDuration / totalTrips}
        addons={durationAddons}
      />
      <RowStat
        label={"DescentTime"}
        value={descentDuration / totalTrips}
        addons={durationAddons}
      />

      <RowStat label={"Peaks"} value={totalPeaks / totalTrips} />
      <RowStat label={"Regions"} value={regionsVisited / totalTrips} />
    </StatsCard>
  );
}

const distanceAddons = {
  formatt: formatter.toKm,
  unit: "km",
} satisfies StatAddons;

const durationAddons = {
  formatt: formatter.toHours,
  unit: "hrs",
} satisfies StatAddons;

const toMinutes = (duration: string) => {
  return Number(formatDuration.toMinutes(duration)) ?? 0;
};
