import { Chart, useChart } from "@chakra-ui/charts";
import {
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Line,
  LineChart,
} from "recharts";
import type { ChartProps } from "./BarGraph";

function ElevationChart({ chartConfig, styleProps }: ChartProps) {
  const chart = useChart(chartConfig);

  const formatX = (value: number) =>
    chart.formatNumber({
      style: "unit",
      unit: "kilometer",
      unitDisplay: "narrow",
      maximumFractionDigits: 1,
    })(value / 1000);

  return (
    <Chart.Root {...styleProps} chart={chart}>
      <LineChart data={chart.data}>
        <CartesianGrid stroke={chart.color("border")} vertical={false} />
        <XAxis
          axisLine={false}
          dataKey={chart.key("dist")}
          tickFormatter={formatX}
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
            key={item.name as string}
            isAnimationActive={true}
            dataKey={chart.key(item.name as string)}
            stroke={chart.color(item.color)}
            strokeWidth={2}
            dot={false}
          />
        ))}
      </LineChart>
    </Chart.Root>
  );
}
export default ElevationChart;
