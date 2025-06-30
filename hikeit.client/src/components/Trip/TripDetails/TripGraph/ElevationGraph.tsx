import { LazyLineGraph } from "@/components/Graphs";
import { Flex, Skeleton } from "@chakra-ui/react";
import { Suspense, memo } from "react";
import { GenerateChartData } from "./GenerateChartData";
import type { ChartData } from "./_graph_types";

interface Props {
  data: ChartData;
}

export function ElevationGraph({ data }: Props) {
  const { gains, start } = data;

  const chartPoints = GenerateChartData(gains, start);

  const styles = {
    width: "80vw",
    height: "60vh",
  };

  return (
    <Flex gapX={8}>
      <Suspense fallback={<Skeleton {...styles} />}>
        <LazyLineGraph
          chartConfig={{
            data: chartPoints,
            series: [{ name: "ele", color: "gray.blue" }],
          }}
          styleProps={styles}
        />
      </Suspense>
    </Flex>
  );
}
export default memo(ElevationGraph);
