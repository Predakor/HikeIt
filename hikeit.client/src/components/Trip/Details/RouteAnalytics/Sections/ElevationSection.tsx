import { IconArrowDown, IconArrowUp, IconTrendUp } from "@/Icons/Icons";
import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import { Flex, Stack } from "@chakra-ui/react";
import type { RouteAnalyticsProps } from "../RouteAnalytics";

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
            value={GenericFormatter(elevation.lowest)}
            label={"Lowest elevation"}
            addons={{ unit: "m", IconSource: IconArrowDown }}
          />
          <RowStat
            value={GenericFormatter(elevation.highest)}
            label={"Highest Elevation"}
            addons={{ unit: "m", IconSource: IconArrowUp }}
          />
        </Flex>

        <RowStat
          value={GenericFormatter(elevation.delta)}
          label={"Total Elevation Delta"}
          addons={{ unit: "m", IconSource: IconTrendUp }}
        />
      </Stack>
    </SimpleCard>
  );
}
