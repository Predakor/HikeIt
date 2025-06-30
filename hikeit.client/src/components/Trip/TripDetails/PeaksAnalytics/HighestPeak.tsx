import type { ReachedPeakWithBadges } from "@/components/AddFile/AddFile/tripTypes";
import RowStat from "@/components/Stats/RowStat";
import { Stack } from "@chakra-ui/react";

export function HighestPeak({ highest }: { highest: ReachedPeakWithBadges }) {
  return (
    <Stack
      direction={{ base: "column", lg: "row" }}
      alignItems={{ base: "start", lg: "center" }}
      gapY={{ base: 8, lg: 4 }}
    >
      <RowStat value={highest.name} label={"name"} />
      <RowStat value={highest.height} addons={{ unit: "m" }} label={"height"} />

      {highest.reachedAt && (
        <RowStat value={highest.reachedAt} label={"reached at"} />
      )}
    </Stack>
  );
}
