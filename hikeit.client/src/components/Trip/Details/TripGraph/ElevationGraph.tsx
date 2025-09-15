import { LazyLineGraph } from "@/components/Graphs";
import { Skeleton } from "@chakra-ui/react";
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
    <Suspense fallback={<Skeleton {...styles} />}>
      <LazyLineGraph
        aspectRatio={"4"}
        config={{
          data: chartPoints,
          series: [{ name: "ele", color: "blue" }],
        }}
      />
    </Suspense>
  );
}
export default memo(ElevationGraph);
