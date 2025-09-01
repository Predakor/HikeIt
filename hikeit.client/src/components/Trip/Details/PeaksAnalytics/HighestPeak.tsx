import RowStat from "@/components/Stats/RowStat";
import type { ReachedPeakWithBadges } from "@/components/ui/Inputs/File/trip.types";
import { SimpleGrid } from "@chakra-ui/react";

export function HighestPeak({ highest }: { highest: ReachedPeakWithBadges }) {
  return (
    <SimpleGrid columns={{ lg: 2 }} gap={{ base: 4, lg: 8 }}>
      <RowStat value={highest.name} label={"name"} />
      <RowStat value={highest.height} addons={{ unit: "m" }} label={"height"} />

      {highest.reachedAt && (
        <RowStat value={highest.reachedAt} label={"reached at"} />
      )}
    </SimpleGrid>
  );
}
