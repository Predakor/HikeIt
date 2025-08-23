import {
  GenericFormatter,
  KeyToLabelFormatter,
} from "@/Utils/Formatters/valueFormatter";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import { LazyBarGraph } from "@/components/Graphs";
import RowStat from "@/components/Stats/RowStat";
import { formatter } from "@/components/User/Stats/Utils/formatter";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { RouteAnalytic } from "@/types/ApiTypes/Analytics";
import { SimpleGrid, Skeleton, Stack } from "@chakra-ui/react";
import { Suspense } from "react";

export default function RouteAnalytics({ data }: { data: RouteAnalytic }) {
  console.log(data);

  const distanceStats = {
    total: data.totalDistanceMeters,
    ascent: data.totalAscentMeters,
    descent: data.totalDescentMeters,
  };
  const elevation = {
    lowest: data.lowestElevationMeters,
    highest: data.highestElevationMeters,
  };

  return (
    <SimpleGrid columns={{ base: 1, lg: 3 }} gapY={8}>
      <SimpleCard title="Distance">
        <Stack gapY={8}>
          <RowStat
            label="Total Distance "
            value={data.totalDistanceMeters}
            addons={{ unit: "km", formatt: formatter.toKm }}
          />
          <Suspense fallback={<Skeleton height={"24"} />}>
            <LazyBarGraph
              chartConfig={{
                data: [
                  {
                    name: "Ascent",
                    color: "blue",
                    value: distanceStats.ascent,
                  },
                  {
                    name: "Descent",
                    color: "purple",
                    value: distanceStats.descent,
                  },
                ],
              }}
            />
          </Suspense>
        </Stack>
      </SimpleCard>
      <SimpleCard title="elevation">
        <MapToStats data={elevation} />
      </SimpleCard>
    </SimpleGrid>
  );
}

function MapToStats({ data }: { data: object }) {
  console.log(data);

  return ObjectToArray(data).map(([name, value]) => (
    <RowStat
      value={GenericFormatter(value)}
      label={KeyToLabelFormatter(name)}
      key={name}
    />
  ));
}
