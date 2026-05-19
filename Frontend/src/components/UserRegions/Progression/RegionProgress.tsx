import RegionHighestPeak from "@/components/Regions/HighestPeak/RegionHighestPeak";
import RegionPeaks from "@/components/Regions/Peaks/RegionPeaks";
import RegionStats from "@/components/Regions/Stats/RegionStats";
import { GoBackButton } from "@/components/ui/Buttons";
import PageTitle from "@/components/ui/Titles/PageTitle";
import type { RegionProgressFull } from "@/types/Api/region.types";
import { Grid, GridItem, Flex } from "@chakra-ui/react";

export default function RegionProgress({ data }: { data: RegionProgressFull }) {
  return (
    <Grid
      columns={{
        base: 1,
        lg: 2,
      }}
      templateRows={{
        lg: "min-content",
      }}
      flexGrow={1}
      gap={8}
    >
      <GridItem
        colSpan={{
          base: 1,
          lg: 2,
        }}
      >
        <Flex>
          <GoBackButton />
          <PageTitle title={data.region.name} />
        </Flex>
      </GridItem>

      <GridItem
        colStart={{
          base: 1,
          lg: 2,
        }}
      >
        <RegionHighestPeak peak={data.highestPeak} />
      </GridItem>

      <GridItem
        colStart={{
          base: 1,
          lg: 2,
        }}
      >
        <RegionStats data={data} />
      </GridItem>

      <GridItem
        rowSpan={2}
        rowStart={{
          base: 4,
          lg: 2,
        }}
      >
        <RegionPeaks peaks={data.peaks} />
      </GridItem>
    </Grid>
  );
}
