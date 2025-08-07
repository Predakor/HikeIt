import { ObjectToArray } from "@/Utils/ObjectToArray";
import RenderStats from "../RenderStats";
import StatsCard from "../StatsCard";
import type { Locations } from "../Utils/statTypes";

export function LocationStats({ stats }: { stats: Locations }) {
  return (
    <StatsCard title={"Locations"}>
      <RenderStats stats={ObjectToArray(stats)} />
    </StatsCard>
  );
}
