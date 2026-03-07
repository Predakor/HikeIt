import {
  IconArrowDown,
  IconArrowUp,
  IconTrendDown,
  IconTrendUp,
} from "@/Icons/Icons";
import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { ComparedStat } from "@/components/Stats";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import { Flex, Stack, Strong, Text } from "@chakra-ui/react";
import type { RouteAnalyticsProps } from "../RouteAnalytics";
import { BarGraph } from "@/components/Graphs";

export function ElevationSection({ data }: RouteAnalyticsProps) {
  const elevation = {
    lowest: data.lowestElevationMeters,
    highest: data.highestElevationMeters,
    delta: data.highestElevationMeters - data.lowestElevationMeters,
  };

  return (
    <SimpleCard title="elevation">
      <Stack gapY={8}>
        <Flex>
          <RowStat
            label="Total Ascent"
            value={data.totalAscentMeters}
            addons={{
              ...sharedAddons,
              IconSource: IconTrendUp,
            }}
          />
          <RowStat
            label="Total Descent"
            value={data.totalDescentMeters}
            addons={{
              ...sharedAddons,
              IconSource: IconTrendDown,
            }}
          />
        </Flex>

        <Flex>
          <RowStat
            value={elevation.highest}
            label={"Highest Elevation"}
            addons={{
              ...sharedAddons,
              IconSource: IconArrowUp,
            }}
          />
          <RowStat
            value={elevation.lowest}
            label={"Lowest elevation"}
            addons={{
              ...sharedAddons,
              IconSource: IconArrowDown,
            }}
          />
        </Flex>
        <BarGraph
          formatValue={(t) => GenericFormatter(t as number)}
          unit="m"
          items={[
            {
              name: "Ascent",
              color: "green.600",
              value: data.totalAscentMeters,
            },
            {
              name: "Descent",
              color: "red.600",
              value: data.totalDescentMeters,
            },
          ]}
        />

        <Stack
          direction={{ base: "column", "2xl": "row" }}
          justify="space-around"
          textAlign={"center"}
          gapY={4}
        >
          <ComparedStat
            title="Aseneded"
            stat={data.totalAscentMeters}
            {...sharedComparedStats}
          />
          <ComparedStat
            title="Descended"
            stat={data.totalDescentMeters}
            {...sharedComparedStats}
          />
        </Stack>
        <Text
          fontSize={"lg"}
          textAlign={"center"}
          css={{
            "& strong": { fontSize: "larger", color: "gray.200" },
            "& span": { fontSize: "smaller", color: "gray.400" },
          }}
        >
          <span>From lowest to highest point</span>
          <br />
          ascended <Strong>{elevation.delta.toFixed()}</Strong> meters that's
          about
          <br />
          <span>
            <Strong>{toFloors(elevation.delta)}</Strong> floors or{" "}
            <Strong>{toBurhKhalifas(elevation.delta)}</Strong> Burj Khalifas
          </span>
        </Text>
      </Stack>
    </SimpleCard>
  );
}

const toBurhKhalifas = (elvation: number) =>
  (toFloors(elvation) / burjKhalifaFloors).toFixed(2);

const toFloors = (elevation: number) =>
  Math.round(elevation / averageFloorHeight);

const averageFloorHeight = 3;
const burjKhalifaFloors = 163;

const sharedAddons = {
  unit: "m",
  formatt: GenericFormatter,
} as const;

const sharedComparedStats = {
  helperText: "worth of elevation",
  unit: {
    name: "floors",
    ratio: averageFloorHeight,
  },
} as const;
