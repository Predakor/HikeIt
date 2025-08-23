import RowStat from "@/components/Stats/RowStat";
import type { PeakSummaryData } from "@/types/ApiTypes/Analytics";
import { Stack } from "@chakra-ui/react";

export function PeakSummary({ summary }: { summary: PeakSummaryData }) {
  return (
    <Stack direction={{ base: "column", lg: "row" }}>
      <RowStat value={summary.total} label={"total peaks"} />
      <RowStat value={summary.unique} label={"Unique peaks"} />
      <RowStat value={summary.new} label={"First Summits"} />
    </Stack>
  );
}
