import { ObjectToArray } from "@/Utils/ObjectToArray";
import RowStat from "@/components/Stats/RowStat";
import RenderStats from "../RenderStats";
import StatsCard from "../Shared/StatsCard";
import { formatter } from "../Utils/formatter";
import type { Metas } from "../Utils/statTypes";

export function MetadatesStats({ metas }: { metas: Metas }) {
  const { longestTripDistanceMeters, longestTripDuration, ...rest } = metas;

  return (
    <StatsCard title="Trip metadatas" columns={2}>
      <RenderStats stats={ObjectToArray(rest)} />

      <RowStat
        label="Furthest Trip"
        value={longestTripDistanceMeters}
        addons={{ formatt: formatter.toKm, unit: "km" }}
      />

      <RowStat
        label="Longest Trip"
        value={longestTripDuration}
        addons={{ formatt: formatter.toDuration }}
      />
    </StatsCard>
  );
}
