import { Chart, useChart } from "@chakra-ui/charts";
import { Flex } from "@chakra-ui/react";
import { useState } from "react";
import {
  CartesianGrid,
  Line,
  LineChart,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
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
  const chart = useChart({
    data: chartPoints,
    series: [
      { name: "ele", color: "gray.subtle" },
      { name: "previewEle", color: "red" },
    ],
  });

  return (
    <Flex gapX={8}>
      <Chart.Root minW={"80vw"} w={"full"} maxH={"lg"} chart={chart}>
        <LineChart data={chart.data}>
          <CartesianGrid stroke={chart.color("border")} vertical={false} />
          <XAxis
            axisLine={false}
            dataKey={chart.key("dist")}
            tickFormatter={
              (value: number) =>
                chart.formatNumber({
                  style: "unit",
                  unit: "kilometer",
                  unitDisplay: "narrow",
                  maximumFractionDigits: 1,
                })(value / 1000) // ← convert meters to kilometers
            }
            stroke={chart.color("border")}
          />
          <YAxis
            axisLine={false}
            tickLine={false}
            tickMargin={10}
            dataKey={chart.key("ele")}
            stroke={chart.color("blue")}
          />
          <Tooltip
            animationDuration={100}
            cursor={false}
            content={<Chart.Tooltip />}
          />
          {chart.series.map((item) => (
            <Line
              key={item.name}
              isAnimationActive={true}
              dataKey={chart.key(item.name)}
              stroke={chart.color(item.color)}
              strokeWidth={2}
              dot={false}
            />
          ))}
        </LineChart>
      </Chart.Root>
      <DevConfig onSubmit={(data: ChartData) => setPreview(data.gains)} />
    </Flex>
  );
}

export default PreviewGraphDevOnly;
