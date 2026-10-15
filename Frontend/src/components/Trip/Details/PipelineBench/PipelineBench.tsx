import type { StageId, StageResults } from "@/types/Api/pipelinePreview.types";
import {
  Box,
  Button,
  Flex,
  Heading,
  SegmentGroup,
  SimpleGrid,
  Stack,
  Tabs,
  Text,
} from "@chakra-ui/react";
import type { ReactNode } from "react";
import { useState } from "react";
import { useParams } from "react-router";
import { usePipelineDefaults } from "./api/usePipelineDefaults";
import { AnalyticsBlock } from "./blocks/AnalyticsBlock";
import { CleanupBlock } from "./blocks/CleanupBlock";
import { PeaksBlock } from "./blocks/PeaksBlock";
import { ProfileBlock } from "./blocks/ProfileBlock";
import { RouteBlock } from "./blocks/RouteBlock";
import { StatsTable } from "./results/StatsTable";
import { usePipelineBench } from "./state/usePipelineBench";
import ElevationOverlayGraph from "./views/ElevationOverlayGraph";
import PeaksList from "./views/PeaksList";
import RoutePreviewMap from "./views/RoutePreviewMap";
type BaselineSource = "config" | "raw";

const baselineItems: { value: BaselineSource; label: string }[] = [
  { value: "config", label: "Current config" },
  { value: "raw", label: "Raw file" },
];

// Every config block gets the same height (scrolls when its filters do not fit), so the cards line up in the grid.
const configBlockHeight = "22rem";

interface StageCardProps {
  stage: keyof typeof stageLists;
  baseline: StageResults;
  preview: StageResults;
  children: ReactNode;
}

// Stages whose rows sit in the results column next to their configs.
const leftColumnStages: StageId[] = ["cleanup", "analytics", "peakMatching"];

// Stable per-stage arrays, so the memoised stats table does not re-render when the bench does.
const stageLists: Record<"elevationProfile" | "route", StageId[]> = {
  elevationProfile: ["elevationProfile"],
  route: ["route"],
};

// One stage: its fixed-height config block with that stage's rows of the stats table right below it.
function StageCard({ stage, baseline, preview, children }: StageCardProps) {
  return (
    <Stack gap={3} minW={0}>
      <Box h={configBlockHeight} overflowY={"auto"}>
        {children}
      </Box>
      <StatsTable stages={stageLists[stage]} baseline={baseline} preview={preview} />
    </Stack>
  );
}

// Admin-only pipeline bench: cleanup / analytics / peaks configs with their results beside them, elevation and
// route as config + stats cards, and the elevation / route / peaks previews in tabs across the full width below.
export default function PipelineBench() {
  const { tripId } = useParams();
  const { state, updateConfig, run } = usePipelineBench(tripId);
  const defaults = usePipelineDefaults(tripId);
  const { config, status, results, raw } = state;
  const [baselineSource, setBaselineSource] = useState<BaselineSource>("config");
  // What the preview is compared against: the current (DB) config run, or the untouched file.
  const baseline = baselineSource === "raw" ? raw : state.baseline;

  return (
    <Stack gap={6}>
      <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
        <Heading size={"md"}>Pipeline bench</Heading>

        <Flex align={"center"} gap={3} wrap={"wrap"}>
          <Text as="span" fontSize={"sm"} color={"fg.muted"}>
            Baseline:
          </Text>
          <SegmentGroup.Root
            size={"sm"}
            value={baselineSource}
            onValueChange={(details) => {
              if (details.value === null) {
                return;
              }

              setBaselineSource(details.value as BaselineSource);
            }}
          >
            <SegmentGroup.Indicator />
            <SegmentGroup.Items items={baselineItems} />
          </SegmentGroup.Root>

          <Button size={"sm"} variant={"outline"} onClick={() => run()}>
            Run all stale
          </Button>
        </Flex>
      </Flex>

      {/* Cleanup, analytics and peak matching: configs on the left, their results on the right. */}
      <Stack direction={{ base: "column", lg: "row" }} gap={6} align={"start"}>
        <Stack flex={1} minW={0} gap={4} w={"full"}>
          <CleanupBlock
            status={status.cleanup}
            result={results.cleanup}
            filters={config.cleanup?.filters}
            defaultFilters={defaults.cleanup}
            onFiltersChange={(filters) => updateConfig("cleanup", { filters })}
            onRun={() => run("cleanup")}
          />

          <AnalyticsBlock
            status={status.analytics}
            result={results.analytics}
            onRun={() => run("analytics")}
          />

          <PeaksBlock
            status={status.peakMatching}
            result={results.peakMatching}
            filters={config.peakMatching?.filters}
            proximityRadiusM={config.peakMatching?.proximityRadiusM}
            defaultFilters={defaults.peaks}
            defaultProximityRadiusM={defaults.peakProximityRadiusM}
            onFiltersChange={(filters) => updateConfig("peakMatching", { filters })}
            onProximityRadiusChange={(proximityRadiusM) =>
              updateConfig("peakMatching", { proximityRadiusM })
            }
            onRun={() => run("peakMatching")}
          />
        </Stack>

        <Stack flex={1} minW={0} w={"full"}>
          <StatsTable stages={leftColumnStages} baseline={baseline} preview={results} />
        </Stack>
      </Stack>

      {/* Elevation profile and route: fixed-height config with its own stats below. */}
      <SimpleGrid columns={{ base: 1, lg: 2 }} gap={6} alignItems={"start"}>
        <StageCard stage={"elevationProfile"} baseline={baseline} preview={results}>
          <ProfileBlock
            status={status.elevationProfile}
            result={results.elevationProfile}
            filters={config.elevationProfile?.filters}
            defaultFilters={defaults.elevationProfile}
            onFiltersChange={(filters) => updateConfig("elevationProfile", { filters })}
            onRun={() => run("elevationProfile")}
          />
        </StageCard>

        <StageCard stage={"route"} baseline={baseline} preview={results}>
          <RouteBlock
            status={status.route}
            result={results.route}
            filters={config.route?.filters}
            defaultFilters={defaults.route}
            onFiltersChange={(filters) => updateConfig("route", { filters })}
            onRun={() => run("route")}
          />
        </StageCard>
      </SimpleGrid>

      {/* Previews take the full width below the cards. */}
      <Tabs.Root defaultValue={"elevation"} lazyMount={true}>
        <Tabs.List>
          <Tabs.Trigger value={"elevation"}>Elevation</Tabs.Trigger>
          <Tabs.Trigger value={"route"}>Route</Tabs.Trigger>
          <Tabs.Trigger value={"peaks"}>Peaks</Tabs.Trigger>
        </Tabs.List>

        <Tabs.Content value={"elevation"}>
          <ElevationOverlayGraph
            preview={results.elevationProfile?.data ?? null}
            baseline={baseline.elevationProfile?.data ?? null}
          />
        </Tabs.Content>

        <Tabs.Content value={"route"}>
          <RoutePreviewMap
            preview={results.route?.data ?? null}
            baseline={baseline.route?.data ?? null}
          />
        </Tabs.Content>

        <Tabs.Content value={"peaks"}>
          <PeaksList
            preview={results.peakMatching?.data ?? null}
            baseline={baseline.peakMatching?.data ?? null}
          />
        </Tabs.Content>
      </Tabs.Root>
    </Stack>
  );
}
