import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { TimeAnalytic } from "@/types/ApiTypes/analytics.types";
import { Flex } from "@chakra-ui/react";
import type { Duration } from "../../TimeAnalytics";
import { IconPause, IconPlay } from "@/Icons/Icons";

interface Props {
  data: TimeAnalytic;
  duration: Duration;
}

export function TimeframeSection({ data }: Props) {
  const tripStart = new Date(data.startTime).toLocaleTimeString();
  const tripEnd = new Date(data.endTime).toLocaleTimeString();
  return (
    <SimpleCard title="TimeFrame">
      <Flex>
        <RowStat
          label="Start"
          value={tripStart}
          addons={{ IconSource: IconPlay }}
        />
        <RowStat
          label="End"
          value={tripEnd}
          addons={{ IconSource: IconPause }}
        />
      </Flex>
    </SimpleCard>
  );
}
