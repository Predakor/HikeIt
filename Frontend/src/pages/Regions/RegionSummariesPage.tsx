import SkeletonGrid from "@/components/Placeholders/SkeletonGrid";
import ProgressedRegion from "@/components/Regions/Card/ProgressedRegion";
import UnprogressedRegion from "@/components/Regions/Card/UnprogressedRegion";
import PageTitle from "@/components/ui/Titles/PageTitle";
import SubTitle from "@/components/ui/Titles/SubTitle";
import FetchWrapper from "@/components/Utils/Fetching";
import UseRegionsProgressions from "@/hooks/Regions/UseRegionsProgressions";
import usePagePreload from "@/hooks/Utils/usePagePreload";
import type { Region, RegionProgressSummary } from "@/types/Api/region.types";
import { For, GridItem, Show, SimpleGrid, Stack } from "@chakra-ui/react";

function RegionsPage() {
  usePagePreload("regions/:regionId");
  const regionsSummaries = UseRegionsProgressions();

  return (
    <Stack gap={10}>
      <PageTitle title="Regions" />
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
                  <SubTitle color={"fg.muted"} title="Visited" />
                </GridItem>
                <ProgressedRegions regions={progressedRegions} />
              </Show>
              <Show when={unprogressedRegions.length}>
                <GridItem colSpan={4}>
                  <SubTitle color={"fg.muted"} title="Unvisited" />
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

function ProgressedRegions({ regions }: { regions: RegionProgressSummary[] }) {
  return (
    <For each={regions}>
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
