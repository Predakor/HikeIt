import GoBackButton from "@/components/ui/Buttons/GoBackButton";
import RegionHighestPeak from "@/components/Regions/HighestPeak/RegionHighestPeak";
import RegionPeaks from "@/components/Regions/Peaks/RegionPeaks";
import RegionStats from "@/components/Regions/Stats/RegionStats";
import PageTitle from "@/components/Titles/PageTitle";
import FetchWrapper from "@/components/Wrappers/Fetching";
import UseRegionProgress from "@/hooks/Regions/UseRegionProgress";
import { Flex, Grid, GridItem } from "@chakra-ui/react";

function RegionProgressionsPage() {
  const getRegionProgress = UseRegionProgress();

  return (
    <FetchWrapper request={getRegionProgress}>
      {(progress) => (
        <Grid columns={{ base: 1, lg: 2 }} gap={4}>
          <GridItem colSpan={{ base: 1, lg: 2 }}>
            <Flex>
              <GoBackButton />
              <PageTitle title={progress.region.name} />
            </Flex>
          </GridItem>

          <GridItem colStart={{ base: 1, lg: 2 }}>
            <RegionHighestPeak peak={progress.highestPeak} />
          </GridItem>

          <GridItem colStart={{ base: 1, lg: 2 }}>
            <RegionStats data={progress} />
          </GridItem>

          <GridItem rowSpan={2} rowStart={{ base: 4, lg: 2 }}>
            <RegionPeaks peaks={progress.peaks} />
          </GridItem>
        </Grid>
      )}
    </FetchWrapper>
  );
}

export default RegionProgressionsPage;
