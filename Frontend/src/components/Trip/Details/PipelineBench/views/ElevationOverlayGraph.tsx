import { LazyLineGraph } from "@/components/Graphs";
import { Skeleton, Stack, Text } from "@chakra-ui/react";
import { Suspense, memo, useMemo } from "react";
import type { ElevationOverlayGraphProps } from "../bench.types";
import { mergeProfiles } from "./mergeProfiles";

const styles = {
  height: "60vh",
};

function ElevationOverlayGraph({ baseline, preview }: ElevationOverlayGraphProps) {
  const merged = useMemo(() => mergeProfiles(baseline, preview), [baseline, preview]);

  const hasBaseline = merged.baselinePointCount > 0;
  const hasPreview = merged.previewPointCount > 0;

  // Memoised so an unrelated re-render of the bench does not hand the chart a new config and redraw every point.
  const chartConfig = useMemo(
    () => ({
      data: merged.points,
      series: [
        ...(hasBaseline ? [{ name: "baselineEle", color: "gray.solid" }] : []),
        ...(hasPreview ? [{ name: "previewEle", color: "red" }] : []),
      ],
    }),
    [merged.points, hasBaseline, hasPreview],
  );

  if (!hasBaseline && !hasPreview) {
    return <Text color={"fg.muted"}>No data</Text>;
  }

  return (
    <Stack gap={2}>
      <Suspense fallback={<Skeleton {...styles} />}>
        <LazyLineGraph {...styles} config={chartConfig} />
      </Suspense>
      <Text fontSize={"sm"} color={"fg.muted"}>
        {`Baseline points: ${merged.baselinePointCount} · Preview points: ${merged.previewPointCount}`}
      </Text>
    </Stack>
  );
}

export default memo(ElevationOverlayGraph);
