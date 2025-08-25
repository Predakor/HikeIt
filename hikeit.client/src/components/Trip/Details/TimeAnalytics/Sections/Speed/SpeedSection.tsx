import { IconArrowDown, IconArrowUp, IconHiking } from "@/Icons/Icons";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { TimeAnalytic } from "@/types/ApiTypes/analytics.types";
import { Flex } from "@chakra-ui/react";
import { sharedAddons } from "../../TimeAnalytics";

export function SpeedSection({ data }: { data: TimeAnalytic }) {
  const speeds = {
    average: data.averageSpeedKph,
    ascent: data.averageAscentKph, //vertical gain only
    descent: data.averageDescentKph, //vertical lose only
  };

  return (
    <SimpleCard title="Speeds">
      <Flex>
        <RowStat
          label="Average speed"
          value={speeds.average}
          addons={{
            ...sharedAddons,
            IconSource: IconHiking,
          }}
        />
      </Flex>

      <Flex>
        <RowStat
          label="Vertical Ascent "
          value={speeds.ascent}
          addons={{ ...sharedAddons, IconSource: IconArrowUp }}
        />
        <RowStat
          label="Vertical Descent "
          value={speeds.descent}
          addons={{
            ...sharedAddons,
            IconSource: IconArrowDown,
          }}
        />
      </Flex>
    </SimpleCard>
  );
}
