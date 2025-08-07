import { ObjectToArray } from "@/Utils/ObjectToArray";
import RowStat from "@/components/Stats/RowStat";
import type { Metas } from "../Utils/statTypes";
import { formatter } from "../Utils/formatter";
import RenderStats from "../RenderStats";
import StatsCard from "../StatsCard";

export function MetasStats({ metas }: { metas: Metas }) {
  const { longestTripMeters, ...rest } = metas;
  const __mockupLongestTrip = "00:34:21";

  return (
    <StatsCard title="Metas">
      <RenderStats stats={ObjectToArray(rest)} />

      <RowStat
        label="Furthest Trip"
        value={longestTripMeters}
        addons={{ formatt: formatter.toKm, unit: "km" }}
      />
      <RowStat
        label="Longest Trip"
        value={__mockupLongestTrip}
        addons={{ formatt: formatter.toDuration }}
      />
    </StatsCard>
  );
}
