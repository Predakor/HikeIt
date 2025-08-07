import { ObjectToArray } from "@/Utils/ObjectToArray";
import type { Totals } from "../Utils/statTypes";
import RenderStats from "../RenderStats";
import StatsCard from "../StatsCard";

export function TotalsStats({ stats }: { stats: Totals }) {
  const statsArray = ObjectToArray(stats);

  return (
    <StatsCard title="Totals">
      <RenderStats stats={statsArray} />
    </StatsCard>
  );
}
