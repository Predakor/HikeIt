import { BarSegment, useChart, type UseChartProps } from "@chakra-ui/charts";
import type { SystemStyleObject } from "@chakra-ui/react";

export interface ChartProps {
  chartConfig: UseChartProps<any>;
  styleProps?: SystemStyleObject;
}

function BarGraph({ chartConfig, styleProps }: ChartProps) {
  const chart = useChart(chartConfig);

  return (
    <BarSegment.Root {...(styleProps as any)} chart={chart}>
      <BarSegment.Content>
        <BarSegment.Value />
        <BarSegment.Bar />
        <BarSegment.Label />
      </BarSegment.Content>
    </BarSegment.Root>
  );
}

export default BarGraph;
