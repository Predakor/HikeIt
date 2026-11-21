import { GenerateChartDataWithPreview } from "@/components/Trip/Details/TripGraph/Dev_PreviewGraph/GenerateChartDataWithPreview";
import { LazyLineGraph } from "@/components/Graphs";
import { Flex, Skeleton, Stack, Text } from "@chakra-ui/react";
import { Suspense, memo, useMemo } from "react";
import type { ElevationOverlayGraphProps } from "./shared/model/bench.types";
import { SeriesLegend } from "./shared/components/SeriesLegend/SeriesLegend";

const compactHeight = "22rem";
const expandedHeight = "60vh";

const baselineColor = "gray.solid";
const previewColor = "red";

// A profile is its start point plus one point per gain.
const pointCountOf = (profile: ElevationOverlayGraphProps["baseline"]) =>
  profile ? profile.gains.length + 1 : 0;

function ElevationOverlayGraph({
  baseline,
  preview,
  isExpanded = false,
}: ElevationOverlayGraphProps) {
  const styles = { height: isExpanded ? expandedHeight : compactHeight };

  const baselinePointCount = pointCountOf(baseline);
  const previewPointCount = pointCountOf(preview);
  const hasBaseline = baselinePointCount > 0;
  const hasPreview = previewPointCount > 0;

  // Memoised so an unrelated re-render of the bench does not hand the chart a new config and redraw every point.
  const chartConfig = useMemo(() => {
    const profile = baseline ?? preview;
    const data =
      profile ?
        GenerateChartDataWithPreview(
          baseline?.gains ?? [],
          profile.start,
          preview?.gains,
          "distance",
        )
      : [];

    return {
      data,
      series: [
        ...(hasBaseline ? [{ name: "ele", color: baselineColor }] : []),
        ...(hasPreview ? [{ name: "previewEle", color: previewColor }] : []),
      ],
    };
  }, [baseline, preview, hasBaseline, hasPreview]);

  if (!hasBaseline && !hasPreview) {
    return <Text color={"fg.muted"}>No data</Text>;
  }

  const legendItems = [
    ...(hasBaseline ? [{ label: "Baseline", color: baselineColor }] : []),
    ...(hasPreview ? [{ label: "Preview", color: previewColor }] : []),
  ];

  return (
    <Stack gap={2}>
      <Suspense fallback={<Skeleton {...styles} />}>
        <LazyLineGraph {...styles} config={chartConfig} />
      </Suspense>
      <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
        <SeriesLegend items={legendItems} />
        <Text fontSize={"sm"} color={"fg.muted"}>
          {`Baseline points: ${baselinePointCount} · Preview points: ${previewPointCount}`}
        </Text>
      </Flex>
    </Stack>
  );
}

export default memo(ElevationOverlayGraph);
