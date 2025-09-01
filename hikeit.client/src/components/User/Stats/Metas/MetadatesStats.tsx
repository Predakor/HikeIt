import type { TimeString } from "@/Utils/Formatters/Duration/Duration";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import RowStat from "@/components/Stats/RowStat";
import RenderStats from "../RenderStats";
import StatsCard from "../Shared/StatsCard";
import { formatter } from "../Utils/formatter";
import type { Metas } from "../Utils/statTypes";

export function MetadatesStats({ metas }: { metas: Metas }) {
  const { longestTripMeters, ...rest } = metas;
  const __mockupLongestTrip = "00:34:21" as TimeString;

  return (
    <StatsCard title="Trip metadatas" columns={2}>
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
