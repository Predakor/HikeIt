import { BarGraph } from "@/components/Graphs";
import type { Duration } from "../../TimeAnalytics";

interface Props {
  duration: Duration;
  total: number;
}

export function TimeDistributionGraph({ duration, total }: Props) {
  const toPercentage = (t: number) => ((t / total) * 100).toFixed() + " %";
  return (
    <BarGraph
      formatValue={toPercentage}
      items={[
        {
          name: "Ascent",
          value: duration.ascent.toSeconds(),
          color: "green.600",
        },
        {
          name: "Descent",
          value: duration.descent.toSeconds(),
          color: "red.600",
        },
        {
          name: "Idle",
          value: duration.iddle.toSeconds(),
          color: "gray.600",
        },
      ]}
    />
  );
}
