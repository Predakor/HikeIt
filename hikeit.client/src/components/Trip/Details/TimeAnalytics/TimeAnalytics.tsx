import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import { BarGraph } from "@/components/Graphs";
import RowStat from "@/components/Stats/RowStat";
import {
  formatter,
  timeConverter,
} from "@/components/User/Stats/Utils/formatter";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { TimeAnalytic } from "@/types/ApiTypes/analytics.types";
import { Flex, SimpleGrid, Stack } from "@chakra-ui/react";

function TimeAnalytics({ data }: { data: TimeAnalytic }) {
  const duration = {
    total: data.duration,
    ascent: data.ascentTime,
    descent: data.descentTime,
    iddle: data.idleTime,
  };

  const speeds = {
    average: data.averageSpeedKph,
    ascent: data.averageAscentKph,
    descent: data.averageDescentKph,
  };

  const tripStart = new Date(data.startTime).toLocaleTimeString();
  const tripEnd = new Date(data.endTime).toLocaleTimeString();

  return (
    <SimpleGrid columns={{ base: 1, lg: 3 }} gap={8}>
      <SimpleCard title="time">
        <Stack gapY={8}>
          <RowStat
            label="Total duration"
            value={duration.total}
            addons={{ formatt: formatter.toDuration }}
          />
          <BarGraph
            formatValue={timeConverter.fromSeconds}
            items={[
              {
                name: "Ascent",
                value: formatter.toRawDuration(duration.ascent),
                color: "green.500",
              },
              {
                name: "Descent",
                value: formatter.toRawDuration(duration.descent),
                color: "red.600",
              },
              {
                name: "Idle",
                value: formatter.toRawDuration(duration.iddle),
                color: "gray.500",
              },
            ]}
          />
        </Stack>
      </SimpleCard>

      <SimpleCard title="TimeFrame">
        <Flex>
          <RowStat label="Start" value={tripStart} />
          <RowStat label="End" value={tripEnd} />
        </Flex>
      </SimpleCard>

      <SimpleCard title="Speeds">
        <Flex>
          {ObjectToArray(speeds).map(([name, value]) => (
            <RowStat
              label={KeyToLabelFormatter(name)}
              value={value}
              addons={{ formatt: (t) => t.toFixed(2) }}
            />
          ))}
        </Flex>
      </SimpleCard>
    </SimpleGrid>
  );
}
export default TimeAnalytics;
