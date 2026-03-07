import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { RegionProgressFull } from "@/types/Api/region.types";
import { Box, SimpleGrid } from "@chakra-ui/react";
import RegionCompletion from "../Completion/RegionCompletion";

export default function RegionStats({ data }: { data: RegionProgressFull }) {
  return (
    <SimpleCard
      title="Peaks progress"
      footer={
        <RegionStatsFooter
          totalPeaks={data.totalPeaksInRegion}
          reachedPeaks={data.uniqueReachedPeaks}
        />
      }
    >
      <SimpleGrid columns={{ base: 2, lg: 3 }} gap={4}>
        <RowStat label="Peaks in region" value={data.totalPeaksInRegion} />
        <RowStat label="Total reached" value={data.totalReachedPeaks} />
        <RowStat label="Unique reached" value={data.uniqueReachedPeaks} />
      </SimpleGrid>
    </SimpleCard>
  );
}

interface Props {
  totalPeaks: number;
  reachedPeaks: number;
}

function RegionStatsFooter({ totalPeaks, reachedPeaks }: Props) {
  return (
    <Box flexGrow={1}>
      <RegionCompletion finished={reachedPeaks} total={totalPeaks} />
    </Box>
  );
}
