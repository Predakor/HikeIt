import { ObjectToArray } from "@/Utils/ObjectToArray";
import { RowStat } from "@/components/Stats";
import RenderStats from "../RenderStats";
import StatsCard from "../Shared/StatsCard";
import type { Locations } from "../Utils/statTypes";

export function LocationStats({ stats }: { stats: Locations }) {
  return (
    <StatsCard title={"Locations"} columns={2}>
      <RowStat label="Total Peaks" value={2} />
      <RowStat label="Total Trips" value={2} />
      <RenderStats stats={ObjectToArray(stats)} />
    </StatsCard>
  );
}
