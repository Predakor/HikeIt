import { LazyLineGraph } from "@/components/Graphs";
import { Flex, Skeleton, Stack } from "@chakra-ui/react";
import { Suspense, memo, useState } from "react";
import type { ChartData, GainDto } from "../_graph_types";
import { DevConfig } from "./DevPreview.tsx";
import { GenerateChartDataWithPreview } from "./GenerateChartDataWithPreview.ts";

interface Props {
  data: ChartData;
}

function PreviewGraphDevOnly({ data }: Props) {
  const { gains, start } = data;

  const [preview, setPreview] = useState<GainDto[]>();

  const chartPoints = GenerateChartDataWithPreview(gains, start, preview);

  const styles = {
    minWidth: "85vw",
    height: "60vh",
  };

  return (
    <Stack direction={{ base: "column", lg: "row" }} gapX={8}>
      <Suspense fallback={<Skeleton {...styles} />}>
        <LazyLineGraph
          {...styles}
          chartConfig={{
            data: chartPoints,
            series: [
              { name: "ele", color: "gray.subtle" },
              { name: "previewEle", color: "red" },
            ],
          }}
        />
      </Suspense>
      <DevConfig onSubmit={(data: ChartData) => setPreview(data.gains)} />
    </Stack>
  );
}

export default memo(PreviewGraphDevOnly);
