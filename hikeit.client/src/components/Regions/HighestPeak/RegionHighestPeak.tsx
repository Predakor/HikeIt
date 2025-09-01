import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { Peak } from "@/types/Api/peak.types";

export default function RegionHighestPeak({ peak }: { peak: Peak }) {
  return (
    <SimpleCard
      title={"Highest peak"}
      bodyStyles={{ direction: "row", align: "center" }}
    >
      <RowStat value={peak.name} label={"name"} />
      <RowStat value={peak.height} addons={{ unit: "m" }} label={"height"} />
    </SimpleCard>
  );
}
