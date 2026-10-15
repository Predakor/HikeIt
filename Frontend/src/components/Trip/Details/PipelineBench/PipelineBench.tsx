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
import { useState } from "react";
import { useParams } from "react-router";
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

// Admin-only pipeline bench: one config block per stage on the left, the stats table on the right,
// and the elevation / route / peaks previews in tabs across the full width below.
export default function PipelineBench() {
  const { tripId } = useParams();
  const { state, updateConfig, run } = usePipelineBench(tripId);
  const { config, status, results, raw } = state;
  const [baselineSource, setBaselineSource] = useState<BaselineSource>("config");
  // What the preview is compared against: the current (DB) config run, or the untouched file.
  const baseline = baselineSource === "raw" ? raw : state.baseline;

  return (
    <Stack gap={8}>
      <Stack direction={{ base: "column", lg: "row" }} gap={6} align={"start"}>
        <Stack flex={1} minW={0} gap={4} w={"full"}>
          <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
            <Heading size={"md"}>Pipeline bench</Heading>
            <Button size={"sm"} variant={"outline"} onClick={() => run()}>
              Run all stale
            </Button>
          </Flex>

          <CleanupBlock
            status={status.cleanup}
            result={results.cleanup}
            filters={config.cleanup?.filters}
            onFiltersChange={(filters) => updateConfig("cleanup", { filters })}
            onRun={() => run("cleanup")}
          />

          <AnalyticsBlock
            status={status.analytics}
            result={results.analytics}
            onRun={() => run("analytics")}
          />

          <RouteBlock
            status={status.route}
            result={results.route}
            filters={config.route?.filters}
            onFiltersChange={(filters) => updateConfig("route", { filters })}
            onRun={() => run("route")}
          />
        </Stack>

        <Stack flex={1} minW={0} gap={4} w={"full"}>
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
          </Flex>

          <StatsTable baseline={baseline} preview={results} />
        </Stack>
      </Stack>

      {/* 2x2 grid: elevation profile and peak matching configs side by side, the previews span both columns below. */}
      <SimpleGrid columns={{ base: 1, lg: 2 }} gap={6} alignItems={"start"}>
        <ProfileBlock
          status={status.elevationProfile}
          result={results.elevationProfile}
          filters={config.elevationProfile?.filters}
          onFiltersChange={(filters) => updateConfig("elevationProfile", { filters })}
          onRun={() => run("elevationProfile")}
        />

        <PeaksBlock
          status={status.peakMatching}
          result={results.peakMatching}
          filters={config.peakMatching?.filters}
          proximityRadiusM={config.peakMatching?.proximityRadiusM}
          onFiltersChange={(filters) => updateConfig("peakMatching", { filters })}
          onProximityRadiusChange={(proximityRadiusM) =>
            updateConfig("peakMatching", { proximityRadiusM })
          }
          onRun={() => run("peakMatching")}
        />

        <Box gridColumn={"1 / -1"} minW={0}>
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
        </Box>
      </SimpleGrid>
    </Stack>
  );
}
