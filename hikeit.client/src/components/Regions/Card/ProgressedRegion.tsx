import RowStat from "@/components/Stats/RowStat";
import type { RegionProgressSummary } from "@/types/ApiTypes/types";
import { Badge, Box, Card, Flex, Stack } from "@chakra-ui/react";
import { Link } from "react-router";
import RegionProgress from "../Completion/RegionCompletion";

interface Props {
  progressSummary: RegionProgressSummary;
}

function ProgressedRegion({ progressSummary }: Props) {
  const { totalPeaksInRegion, uniqueReachedPeaks, region } = progressSummary;

  const isComplete = uniqueReachedPeaks >= totalPeaksInRegion;

  return (
    <Link to={`/regions/${region.id}`}>
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
            <RowStat
              value={uniqueReachedPeaks}
              label={"Unique peaks reached"}
            />
            <RowStat
              value={totalPeaksInRegion}
              label={"Total peaks in region"}
            />
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
    </Link>
  );
}
export default ProgressedRegion;
