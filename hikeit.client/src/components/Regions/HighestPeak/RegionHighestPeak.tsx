import RowStat from "@/components/Stats/RowStat";
import type { Peak } from "@/types/ApiTypes/Analytics";
import { Card, Flex } from "@chakra-ui/react";

export default function RegionHighestPeak({ peak }: { peak: Peak }) {
  return (
    <Card.Root>
      <Card.Header fontSize={{ base: "lg", lg: "2xl" }}>
        Highest peak
      </Card.Header>
      <Card.Body>
        <Flex>
          <RowStat value={peak.name} label={"name"} />
          <RowStat
            value={peak.height}
            addons={{ unit: "m" }}
            label={"height"}
          />
        </Flex>
      </Card.Body>
    </Card.Root>
  );
}
