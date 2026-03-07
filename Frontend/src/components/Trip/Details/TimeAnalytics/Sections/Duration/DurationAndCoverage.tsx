import { IconClock, IconHiking } from "@/Icons/Icons";
import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";
import RowStat from "@/components/Stats/RowStat";
import { timeConverter } from "@/components/User/Stats/Utils/formatter";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import { Grid } from "@chakra-ui/react";
import type { Duration } from "../../TimeAnalytics";

export function DurationAndCoverage({ duration }: { duration: Duration }) {
  //change it after fixing time calculation in api
  //to use routedata.total from parent
  const activeTime = TimeSpan.FromDifference(duration.total, duration.iddle);

  return (
    <SimpleCard title="Duration and Coverage">
      <Grid gridAutoFlow={"column"} placeItems={"center"} gap={4}>
        <RowStat
          label="Total duration"
          value={duration.total.toSeconds()}
          addons={{
            formatt: timeConverter.fromSeconds,
            IconSource: IconClock,
          }}
        />
        <RowStat
          label="Active for"
          value={activeTime.toSeconds() / duration.total.toSeconds()}
          addons={{
            formatt: (t) => (t * 100).toFixed() + "%",
            IconSource: IconHiking,
          }}
        />
      </Grid>
    </SimpleCard>
  );
}
