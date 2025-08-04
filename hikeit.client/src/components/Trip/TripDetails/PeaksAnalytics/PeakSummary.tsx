import { LazyBarGraph } from "@/components/Graphs";
import RowStat from "@/components/Stats/RowStat";
import type { PeakSummaryData } from "@/types/ApiTypes/Analytics";
import { Box, Stack } from "@chakra-ui/react";
import { Suspense } from "react";

export function PeakSummary({ summary }: { summary: PeakSummaryData }) {
  return (
    <>
      <Stack direction={{ base: "column", lg: "row" }}>
        <RowStat value={summary.total} label={"total peaks"} />
        <RowStat value={summary.unique} label={"Unique peaks"} />
        <RowStat value={summary.new} label={"First Summits"} />
      </Stack>
      <Box>
        <Suspense fallback="loading">
          <LazyBarGraph
            chartConfig={{
              sort: { by: "value", direction: "desc" },
              data: [
                {
                  name: "Unique",
                  value: summary.unique,
                  color: "purple.solid",
                },
                {
                  name: "First Summits",
                  value: summary.new,
                  color: "orange",
                },
              ],
            }}
          />
        </Suspense>
      </Box>
    </>
  );
}
