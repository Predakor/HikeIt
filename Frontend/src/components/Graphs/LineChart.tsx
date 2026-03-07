import {
  Chart,
  useChart,
  type ChartRootProps,
  type UseChartProps,
} from "@chakra-ui/charts";
import {
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Line,
  LineChart,
} from "recharts";

interface Props extends Partial<ChartRootProps<any>> {
  config: UseChartProps<any>;
}

function ElevationChart({ config, ...rest }: Props) {
  const chart = useChart(config);

  const formatX = (value: number) =>
    chart.formatNumber({
      style: "unit",
      unit: "kilometer",
      unitDisplay: "narrow",
      maximumFractionDigits: 1,
    })(value / 1000);

  return (
    <Chart.Root {...rest} chart={chart}>
      <LineChart data={chart.data}>
        <CartesianGrid stroke={chart.color("border")} vertical={false} />
        <XAxis
          axisLine={false}
          dataKey={chart.key("dist")}
          tickFormatter={formatX}
          domain={["auto", "auto"]}
          stroke={chart.color("border")}
        />
        <YAxis
          axisLine={false}
          tickLine={false}
          tickMargin={10}
          domain={["auto", "auto"]}
          dataKey={chart.key("ele")}
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
