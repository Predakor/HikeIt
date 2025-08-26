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
      templateColumns={{ lg: "2fr 2fr" }}
      templateRows={{ lg: "2fr 3fr" }}
      gap={8}
    >
      <GridItem colStart={{ lg: 2 }}>
        <TimeAnalyticSection.Duration duration={duration} />
      </GridItem>

      <GridItem colStart={{ lg: -2 }}>
        <TimeAnalyticSection.TimeDistribution duration={duration} />
      </GridItem>

      <GridItem colStart={{ lg: 1 }} rowStart={{ lg: 1 }}>
        <TimeAnalyticSection.TimeFrame data={data} duration={duration} />
      </GridItem>

      <GridItem rowStart={2}>
        <TimeAnalyticSection.Speed data={data} />
      </GridItem>
    </SimpleGrid>
  );
}

export const sharedAddons = {
  formatt: (t: number) => t.toFixed(2),
  unit: "km/h",
} as const;
