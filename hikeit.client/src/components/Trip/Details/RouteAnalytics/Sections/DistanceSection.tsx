import { ComparedStat } from "@/components/Stats";
import RowStat from "@/components/Stats/RowStat";
import { formatter } from "@/components/User/Stats/Utils/formatter";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import { SimpleGrid } from "@chakra-ui/react";
import type { RouteAnalyticsProps } from "../RouteAnalytics";

const marathonLengthMeters = 42200;

export function DistanceSection({ data }: RouteAnalyticsProps) {
  const distance = {
    total: data.totalDistanceMeters,
    ascent: data.totalAscentMeters,
    descent: data.totalDescentMeters,
  };

  return (
    <SimpleCard title="Distance">
      <SimpleGrid
        columns={[1, 2]}
        alignItems={"center"}
        justifyItems={"center"}
        gapY={8}
      >
        <RowStat
          label="Total Distance"
          value={distance.total}
          addons={{
            unit: "km",
            formatt: formatter.toKm,
          }}
        />
        <ComparedStat
          title="Thats"
          helperText="of marathon length"
          stat={distance.total}
          unit={{
            name: "",
            ratio: marathonLengthMeters,
          }}
          options={{ valueIn: "percentile" }}
        />
      </SimpleGrid>
    </SimpleCard>
  );
}
