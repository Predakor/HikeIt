import { Card, Flex, Badge, Box, Stack } from "@chakra-ui/react";
import RegionProgress from "./RegionProgress";
import type { RegionProgressSummary } from "@/data/types";
import RowStat from "@/components/Stats/RowStat";

interface Props {
  progressSummary: RegionProgressSummary;
}

function RegionSummary({ progressSummary }: Props) {
  const { totalPeaksInRegion, uniqueReachedPeaks, region } = progressSummary;

  const isComplete = uniqueReachedPeaks >= totalPeaksInRegion;

  return (
    <Card.Root gapY={4}>
      <Card.Header>
        <Flex>
          <Card.Title flexGrow={1} fontSize={"2xl"}>
            {region.name}
          </Card.Title>
          {isComplete && <Badge colorPalette={"green"}>Complete</Badge>}
        </Flex>
      </Card.Header>
      <Card.Body>
        <Stack direction={{ base: "column", lg: "row" }}>
          <RowStat value={uniqueReachedPeaks} label={"Unique peaks reached"} />
          <RowStat value={totalPeaksInRegion} label={"Total peaks in region"} />
        </Stack>
      </Card.Body>
      <Card.Footer>
        <Box w="100%">
          <RegionProgress
            total={totalPeaksInRegion}
            finished={uniqueReachedPeaks}
          />
        </Box>
      </Card.Footer>
    </Card.Root>
  );
}
export default RegionSummary;
