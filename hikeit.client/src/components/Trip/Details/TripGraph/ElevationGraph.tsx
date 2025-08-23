import { LazyLineGraph } from "@/components/Graphs";
import { Flex, Skeleton } from "@chakra-ui/react";
import { Suspense, memo } from "react";
import { GenerateChartData } from "./GenerateChartData";
import type { ChartData } from "./grap.types";

interface Props {
  data: ChartData;
}

export function ElevationGraph({ data }: Props) {
  const { gains, start } = data;

  const chartPoints = GenerateChartData(gains, start);

  const styles = {
    width: "full",
  };

  return (
    <Flex gapX={8}>
      <Suspense fallback={<Skeleton {...styles} />}>
        <LazyLineGraph
          aspectRatio={"16/9"}
          chartConfig={{
            data: chartPoints,
            series: [{ name: "ele", color: "blue" }],
          }}
          {...styles}
        />
      </Suspense>
    </Flex>
  );
}
export default memo(ElevationGraph);
