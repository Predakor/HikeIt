import { ObjectToArray } from "@/Utils/ObjectToArray";
import RowStat from "@/components/Stats/RowStat";
import type { TripAnalytic } from "@/types/ApiTypes/Analytics";
import { SimpleGrid } from "@chakra-ui/react";

export default function RouteAnalytics({ data }: { data: TripAnalytic }) {
  const entries = ObjectToArray(data);
  return (
    <SimpleGrid columns={{ base: 1, lg: 3 }} gapY={8}>
      {entries.map(([name, value]) => (
        <RowStat value={value} label={name} key={name} />
      ))}
    </SimpleGrid>
  );
}
