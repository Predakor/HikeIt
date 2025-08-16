import SkeletonGrid from "@/components/Placeholders/SkeletonGrid";
import ProgressedRegion from "@/components/Regions/Card/ProgressedRegion";
import UnprogressedRegion from "@/components/Regions/Card/UnprogressedRegion";
import PageTitle from "@/components/Titles/PageTitle";
import FetchWrapper from "@/components/Wrappers/Fetching";
import UseRegionsProgressions from "@/hooks/Regions/UseRegionsProgressions";
import type {
  Region,
  RegionProgressSummary,
} from "@/types/ApiTypes/region.types";
import { For, GridItem, Show, SimpleGrid, Stack } from "@chakra-ui/react";

function RegionsPage() {
  const regionsSummaries = UseRegionsProgressions();

  return (
    <Stack gap={8}>
      <SimpleGrid
        alignItems={"stretch"}
        justifyItems={"stretch"}
        minChildWidth={{ base: "full", lg: "sm" }}
        gap={8}
      >
        <FetchWrapper
          request={regionsSummaries}
          LoadingComponent={SkeletonGrid}
        >
          {([progressedRegions, unprogressedRegions]) => (
            <>
              <Show when={progressedRegions.length}>
                <GridItem colSpan={4}>
                  <PageTitle title="Visited Regions" />
                </GridItem>
                <RegionSummaries summaries={progressedRegions} />
              </Show>
              <Show when={unprogressedRegions.length}>
                <GridItem colSpan={4}>
                  <PageTitle title="Unvisited Regions" />
                </GridItem>
                <UnprogressedRegions regions={unprogressedRegions} />
              </Show>
            </>
          )}
        </FetchWrapper>
      </SimpleGrid>
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
        <ProgressedRegion progressSummary={summary} key={summary.region.id} />
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

export default RegionsPage;
