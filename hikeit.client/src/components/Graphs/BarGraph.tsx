import {
  BarSegment,
  useChart,
  type BarSegmentBarProps,
  type UseChartProps,
} from "@chakra-ui/charts";

export interface ChartProps extends BarSegmentBarProps {
  chartConfig: UseChartProps<any>;
}

function BarGraph({ chartConfig, ...rest }: ChartProps) {
  const chart = useChart(chartConfig);

  return (
    <BarSegment.Root {...rest} chart={chart}>
      <BarSegment.Content>
        <BarSegment.Value />
        <BarSegment.Bar />
        <BarSegment.Label />
      </BarSegment.Content>
    </BarSegment.Root>
  );
}

export default BarGraph;
