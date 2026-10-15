import { Button, Flex, Heading, SegmentGroup, Stack, Text } from "@chakra-ui/react";
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

// Admin-only pipeline bench: cleanup, analytics and peaks on top next to the stats table;
// elevation and route below, each with its full-width preview.
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

          <PeaksList
            preview={results.peakMatching?.data ?? null}
            baseline={baseline.peakMatching?.data ?? null}
          />
        </Stack>
      </Stack>

      {/* Elevation and route: config on top, preview stretched to the full width below it. */}
      <Stack gap={4} w={"full"}>
        <ProfileBlock
          status={status.elevationProfile}
          result={results.elevationProfile}
          filters={config.elevationProfile?.filters}
          onFiltersChange={(filters) => updateConfig("elevationProfile", { filters })}
          onRun={() => run("elevationProfile")}
        />
        <ElevationOverlayGraph
          preview={results.elevationProfile?.data ?? null}
          baseline={baseline.elevationProfile?.data ?? null}
        />
      </Stack>

      <Stack gap={4} w={"full"}>
        <RouteBlock
          status={status.route}
          result={results.route}
          filters={config.route?.filters}
          onFiltersChange={(filters) => updateConfig("route", { filters })}
          onRun={() => run("route")}
        />
        <RoutePreviewMap
          preview={results.route?.data ?? null}
          baseline={baseline.route?.data ?? null}
        />
      </Stack>
    </Stack>
  );
}
