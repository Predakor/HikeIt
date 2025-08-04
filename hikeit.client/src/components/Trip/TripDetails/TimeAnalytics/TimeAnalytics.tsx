import { ObjectToArray } from "@/Utils/ObjectToArray";
import RowStat from "@/components/Stats/RowStat";
import type { TimeAnalytic } from "@/types/ApiTypes/Analytics";
import { SimpleGrid } from "@chakra-ui/react";

function TimeAnalytics({ data }: { data: TimeAnalytic }) {
  const entries = ObjectToArray(data);
  return (
    <SimpleGrid columns={{ base: 1, lg: 3 }} gapY={8}>
      {entries.map(([name, value]) => (
        <RowStat value={value} label={name} />
      ))}
    </SimpleGrid>
  );
}
export default TimeAnalytics;
