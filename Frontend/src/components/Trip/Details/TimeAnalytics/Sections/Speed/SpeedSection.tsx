import {
  IconArrowDown,
  IconArrowUp,
  IconHiking,
  IconTrendDown,
  IconTrendUp,
} from "@/Icons/Icons";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { TimeAnalytic } from "@/types/Api/analytics.types";
import { Stack } from "@chakra-ui/react";
import { sharedAddons } from "../../TimeAnalytics";

export function SpeedSection({ data }: { data: TimeAnalytic }) {
  const speeds = {
    average: data.averageSpeedKph,
    ascent: data.averageAscentKph, //vertical gain only
    descent: data.averageDescentKph, //vertical lose only
  };

  const toMetersPerMin = (v: number) => ((v * 1000) / 60).toFixed(2);

  return (
    <SimpleCard title="Speeds">
      <Stack direction={{ base: "column", lg: "row" }} alignItems={"center"}>
        <RowStat
          label="Average speed"
          value={speeds.average}
          addons={{
            ...sharedAddons,
            IconSource: IconHiking,
          }}
        />
        <RowStat
          label="Climb speed"
          value={speeds.average}
          addons={{
            ...sharedAddons,
            IconSource: IconTrendUp,
          }}
        />
        <RowStat
          label="Descend speed"
          value={speeds.average}
          addons={{
            ...sharedAddons,
            IconSource: IconTrendDown,
          }}
        />
      </Stack>

      <Stack direction={{ base: "column", lg: "row" }} alignItems={"center"}>
        <RowStat
          label="Average Elevation Change "
          value={(speeds.ascent + speeds.descent) / 2}
          addons={{
            unit: "m/min",
            formatt: toMetersPerMin,
            IconSource: IconHiking,
          }}
        />
        <RowStat
          label="Vertical Ascent "
          value={speeds.ascent}
          addons={{
            unit: "m/min",
            formatt: toMetersPerMin,
            IconSource: IconArrowUp,
          }}
        />
        <RowStat
          label="Vertical Descent "
          value={speeds.descent}
          addons={{
            unit: "m/min",
            formatt: toMetersPerMin,
            IconSource: IconArrowDown,
          }}
        />
      </Stack>
    </SimpleCard>
  );
}
