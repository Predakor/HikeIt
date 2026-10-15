import { LazyLineGraph } from "@/components/Graphs";
import { Skeleton, Stack, Text } from "@chakra-ui/react";
import { Suspense, useMemo } from "react";
import type { ElevationOverlayGraphProps } from "../bench.types";
import { mergeProfiles } from "./mergeProfiles";

const styles = {
  height: "60vh",
};

export default function ElevationOverlayGraph({ baseline, preview }: ElevationOverlayGraphProps) {
  const merged = useMemo(() => mergeProfiles(baseline, preview), [baseline, preview]);

  const hasBaseline = merged.baselinePointCount > 0;
  const hasPreview = merged.previewPointCount > 0;

  if (!hasBaseline && !hasPreview) {
    return <Text color={"fg.muted"}>No data</Text>;
  }

  const series = [
    ...(hasBaseline ? [{ name: "baselineEle", color: "gray.solid" }] : []),
    ...(hasPreview ? [{ name: "previewEle", color: "red" }] : []),
  ];

  return (
    <Stack gap={2}>
      <Suspense fallback={<Skeleton {...styles} />}>
        <LazyLineGraph
          {...styles}
          config={{
            data: merged.points,
            series,
          }}
        />
      </Suspense>
      <Text fontSize={"sm"} color={"fg.muted"}>
        {`Baseline points: ${merged.baselinePointCount} · Preview points: ${merged.previewPointCount}`}
      </Text>
    </Stack>
  );
}
