import RegionSummary from "@/components/Regions/Card/RegionSummary";
import UnprogressedRegion from "@/components/Regions/Card/UnprogressedRegion";
import FetchWrapper from "@/components/Wrappers/Fetching";
import type { Region, RegionProgressSummary } from "@/data/types";
import UseRegionsProgressions from "@/hooks/Regions/UseRegionsProgressions";
import {
  Box,
  For,
  Heading,
  SimpleGrid,
  Skeleton,
  Stack,
} from "@chakra-ui/react";

function RegionsPage() {
  const regionsSummaries = UseRegionsProgressions();

  return (
    <Stack gap={8}>
      <Box placeItems={"center"}>
        <Heading size={"5xl"}>Regions</Heading>
      </Box>
      <FetchWrapper
        request={regionsSummaries}
        LoadingComponent={RegionSkeleton}
      >
        {([progressedRegions, unprogressedRegions]) => (
          <SimpleGrid
            alignItems={"stretch"}
            justifyItems={"stretch"}
            minChildWidth={{ base: "full", lg: "sm" }}
            gap={8}
          >
            <RegionSummaries summaries={progressedRegions} />
            <UnprogressedRegions regions={unprogressedRegions} />
          </SimpleGrid>
        )}
      </FetchWrapper>
    </Stack>
  );
}

function RegionSummaries({
  summaries,
}: {
  summaries: RegionProgressSummary[];
}) {
  return (
    <For each={summaries}>
      {(summary) => (
        <RegionSummary progressSummary={summary} key={summary.region.id} />
      )}
    </For>
  );
}

function UnprogressedRegions({ regions }: { regions: Region[] }) {
  return (
    <For each={regions}>
      {(region) => <UnprogressedRegion region={region} key={region.id} />}
    </For>
  );
}

function RegionSkeleton() {
  return (
    <>
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
    </>
  );
}
export default RegionsPage;
