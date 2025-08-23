import { BarGraph } from "@/components/Graphs";
import RowStat from "@/components/Stats/RowStat";
import { formatter } from "@/components/User/Stats/Utils/formatter";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import { Stack } from "@chakra-ui/react";
import type { RouteAnalyticsProps } from "../RouteAnalytics";
import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";

export function DistanceSection({ data }: RouteAnalyticsProps) {
  const distance = {
    total: data.totalDistanceMeters,
    ascent: data.totalAscentMeters,
    descent: data.totalDescentMeters,
  };

  return (
    <SimpleCard title="Distance">
      <Stack gapY={8}>
        <RowStat
          label="Total Distance "
          value={distance.total}
          addons={{
            unit: "km",
            formatt: formatter.toKm,
          }}
        />
        <BarGraph
          formatValue={(v) => v.toFixed()}
          items={[
            {
              name: "Ascent",
              color: "green",
              value: distance.ascent,
            },
            {
              name: "Descent",
              color: "blue",
              value: distance.descent,
            },
          ]}
        />
      </Stack>
    </SimpleCard>
  );
}
