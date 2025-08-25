import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";
import type { TimeAnalytic } from "@/types/ApiTypes/analytics.types";
import { GridItem, SimpleGrid } from "@chakra-ui/react";
import TimeAnalyticSection from "./Sections";

export type Duration = {
  total: TimeSpan;
  ascent: TimeSpan;
  descent: TimeSpan;
  iddle: TimeSpan;
};

export default function TimeAnalytics({ data }: { data: TimeAnalytic }) {
  const duration = {
    total: TimeSpan.FromTimeString(data.duration),
    ascent: TimeSpan.FromTimeString(data.ascentTime),
    descent: TimeSpan.FromTimeString(data.descentTime),
    iddle: TimeSpan.FromTimeString(data.idleTime),
  };

  return (
    <SimpleGrid
      templateColumns={{ lg: "repeat(3,1fr)" }}
      templateRows={{ lg: "repeat(2,1fr)" }}
      gap={8}
    >
      <GridItem colSpan={{ lg: 2 }}>
        <TimeAnalyticSection.Duration duration={duration} />
      </GridItem>

      <GridItem colSpan={{ lg: 1 }} rowSpan={{ lg: 3 }} rowStart={2}>
        <TimeAnalyticSection.TimeDistribution duration={duration} />
      </GridItem>

      <GridItem colSpan={{ lg: 2 }}>
        <TimeAnalyticSection.TimeFrame data={data} duration={duration} />
      </GridItem>

      <GridItem colSpan={{ lg: 2 }}>
        <TimeAnalyticSection.Speed data={data} />
      </GridItem>
    </SimpleGrid>
  );
}

export const sharedAddons = {
  formatt: (t: number) => t.toFixed(2),
  unit: "km/h",
} as const;
