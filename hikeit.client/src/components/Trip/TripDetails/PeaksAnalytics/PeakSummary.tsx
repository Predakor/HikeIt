import { LazyBarGraph } from "@/components/Graphs";
import RowStat from "@/components/Stats/RowStat";
import { Box, Stack } from "@chakra-ui/react";
import { Suspense } from "react";
import type { PeakSummaryData } from "../../Types/TripAnalyticsTypes";

export function PeakSummary({}: { summary: PeakSummaryData }) {
  return (
    <>
      <Stack direction={{ base: "column", lg: "row" }}>
        <RowStat value={5} label={"total peaks"} />
        <RowStat value={3} label={"Unique peaks"} />
        <RowStat value={1} label={"First Summits"} />
      </Stack>
      <Box>
        <Suspense fallback="loading">
          <LazyBarGraph
            chartConfig={{
              sort: { by: "value", direction: "desc" },
              data: [
                { name: "Unique", value: 3, color: "purple.solid" },
                { name: "First Summits", value: 1, color: "orange" },
              ],
            }}
          />
        </Suspense>
      </Box>
    </>
  );
}
