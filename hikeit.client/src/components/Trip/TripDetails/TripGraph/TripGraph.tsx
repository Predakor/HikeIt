import type { GpxArray, GpxPoint } from "@/types/ApiTypes/TripDtos";
import { Chart, useChart } from "@chakra-ui/charts";
import { Box } from "@chakra-ui/react";
import {
  CartesianGrid,
  Line,
  LineChart,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
type GainDto = {
  dist: number;
  ele: number;
  time: number;
};

interface ChartData {
  start: GpxPoint;
  gains: GainDto[];
}

type GpxGainPoint = {
  dist: number;
  ele: number;
  slope: number;
  time?: number;
};

interface Props {
  data: ChartData;
}

export default function ElevationGraph({ data }: Props) {
  const { gains, start } = data;

  const chartPoints: GpxGainPoint[] = new Array(gains.length);

  chartPoints[0] = {
    dist: 0,
    ele: start.ele,
    slope: 0,
    time: 0,
  };

  for (let i = 1; i < chartPoints.length; i++) {
    const prev = chartPoints[i - 1];

    const dist = prev.dist + gains[i].dist;
    const ele = prev.ele + gains[i].ele;

    const current: GpxGainPoint = {
      dist: dist,
      ele: ele,
      slope: ele / dist,
      time: prev?.time || 0 + gains[i]?.time || 0,
    };

    chartPoints[i] = current;
  }

  const chart = useChart({
    data: chartPoints,
    series: [{ name: "ele", color: "blue" }],
  });

  console.log(chart.series);

  return (
    <Box marginTop={"2.5em"}>
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
              isAnimationActive={false}
              dataKey={chart.key(item.name)}
              stroke={chart.color(item.color)}
              strokeWidth={2}
              dot={false}
            />
          ))}
        </LineChart>
      </Chart.Root>
    </Box>
  );
}
