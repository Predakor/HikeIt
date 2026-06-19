import { IconPlay, IconStop } from "@/Icons/Icons";
import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { TimeAnalytic } from "@/types/Api/analytics.types";
import { Flex } from "@chakra-ui/react";
import type { Duration } from "../../TimeAnalytics";
import { TimeInDaySlider } from "./TimeInDaySlider";
import type { TimeSpanString } from "@/types/Api/types";

interface Props {
  data: TimeAnalytic;
  duration: Duration;
}

export function TimeframeSection({ data }: Props) {
  const tripStart = new Date(data.startTime).toLocaleTimeString();
  const tripEnd = new Date(data.endTime).toLocaleTimeString();

  return (
    <SimpleCard title="Time Frame">
      <Flex>
        <RowStat label="Start" value={tripStart} addons={{ IconSource: IconPlay }} />
        <RowStat label="End" value={tripEnd} addons={{ IconSource: IconStop }} />
      </Flex>
      <TimeInDaySlider
        start={TimeSpan.From(tripStart as TimeSpanString)}
        end={TimeSpan.From(tripEnd as TimeSpanString)}
      />
    </SimpleCard>
  );
}
