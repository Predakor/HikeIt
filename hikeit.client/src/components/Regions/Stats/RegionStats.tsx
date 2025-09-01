import RowStat from "@/components/Stats/RowStat";
import type { RegionProgressFull } from "@/types/Api/region.types";
import { Card, Stack, Box } from "@chakra-ui/react";
import RegionCompletion from "../Completion/RegionCompletion";

export default function RegionStats({ data }: { data: RegionProgressFull }) {
  return (
    <Card.Root>
      <Card.Header>
        <Card.Title
          fontSize={{
            base: "lg",
            lg: "2xl",
          }}
        >
          Region progress
        </Card.Title>
      </Card.Header>
      <Card.Body>
        <Stack
          direction={{
            base: "column",
            lg: "row",
          }}
        >
          <RowStat label="Peaks in region" value={data.totalPeaksInRegion} />
          <RowStat label="Total peaks reached" value={data.totalReachedPeaks} />
          <RowStat
            label="Unique peaks reached"
            value={data.uniqueReachedPeaks}
          />
        </Stack>
      </Card.Body>
      <Card.Footer>
        <Box flexGrow={1}>
          <RegionCompletion
            finished={data.uniqueReachedPeaks}
            total={data.totalPeaksInRegion}
          />
        </Box>
      </Card.Footer>
    </Card.Root>
  );
}
