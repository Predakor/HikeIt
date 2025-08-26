import { IconPlay, IconStop } from "@/Icons/Icons";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { TimeAnalytic } from "@/types/ApiTypes/analytics.types";
import { Flex } from "@chakra-ui/react";
import type { Duration } from "../../TimeAnalytics";
import {
  TimeSpan,
  type TimeString,
} from "@/Utils/Formatters/Duration/Duration";
import { TimeInDaySlider } from "./TimeInDaySlider";

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
        <RowStat
          label="Start"
          value={tripStart}
          addons={{ IconSource: IconPlay }}
        />
        <RowStat
          label="End"
          value={tripEnd}
          addons={{ IconSource: IconStop }}
        />
      </Flex>
      <TimeInDaySlider
        start={TimeSpan.From(tripStart as TimeString)}
        end={TimeSpan.From(tripEnd as TimeString)}
      />
    </SimpleCard>
  );
}
