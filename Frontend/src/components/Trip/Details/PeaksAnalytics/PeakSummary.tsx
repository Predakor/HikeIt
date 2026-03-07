import RowStat from "@/components/Stats/RowStat";
import type { PeakSummaryData } from "@/types/Api/analytics.types";
import { Stack } from "@chakra-ui/react";

export function PeakSummary({ summary }: { summary: PeakSummaryData }) {
  return (
    <Stack direction={{ base: "row", lg: "row" }}>
      <RowStat
        styles={{ alignItems: "center" }}
        value={summary.total}
        label={"total peaks"}
      />
      <RowStat
        styles={{ alignItems: "center" }}
        value={summary.unique}
        label={"Unique peaks"}
      />
      <RowStat
        styles={{ alignItems: "center" }}
        value={summary.new}
        label={"First Summits"}
      />
    </Stack>
  );
}
