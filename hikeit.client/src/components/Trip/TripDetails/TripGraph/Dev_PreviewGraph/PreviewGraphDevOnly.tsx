import { LazyLineGraph } from "@/components/Graphs";
import { Flex, Skeleton } from "@chakra-ui/react";
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
    width: "full",
    minWidth: "80vw",
    height: "60vh",
  };

  return (
    <Flex gapX={8}>
      <Suspense fallback={<Skeleton {...styles} />}>
        <LazyLineGraph
          chartConfig={{
            data: chartPoints,
            series: [
              { name: "ele", color: "gray.subtle" },
              { name: "previewEle", color: "red" },
            ],
          }}
          styleProps={styles}
        />
      </Suspense>
      <DevConfig onSubmit={(data: ChartData) => setPreview(data.gains)} />
    </Flex>
  );
}

export default memo(PreviewGraphDevOnly);
