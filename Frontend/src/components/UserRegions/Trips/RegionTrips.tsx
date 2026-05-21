import { IconClock, IconJourney } from "@/Icons/Icons";
import { arrayUtils } from "@/Utils/arrayUtils";
import { RowStat } from "@/components/Stats";
import TripCardRow from "@/components/Trips/Card/TripCardRow";
import { formatter, timeConverter } from "@/components/User/Stats/Utils/formatter";
import FetchWrapper from "@/components/Utils/Fetching";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { RegionTrip } from "@/types/Api/TripDtos";
import type { ResourceUrl } from "@/types/Api/types";
import { For, Stack } from "@chakra-ui/react";
import RegionTripsTimeline from "./Timelines/RegionTripsTimeline";

export default function RegionTrips({ data }: { data: ResourceUrl }) {
  const tripsQuery = useResourceLink<RegionTrip[]>(data);

  return (
    <FetchWrapper request={tripsQuery}>
      {(trips) => {
        const totalDistance = arrayUtils.sum(trips, (x) => x.distance);
        const totalMinutes = arrayUtils.sum(
          trips.filter((x) => x.duration),
          ({ duration }) => timeConverter.toRawDuration(duration!),
        );

        return (
          <Stack gapY={8}>
            <Stack direction={"row"} gap={"4"}>
              <RowStat label={`Total Trips`} value={trips.length} />
              <RowStat
                label={`Total Distance`}
                value={totalDistance}
                addons={{ unit: "km", IconSource: IconJourney, formatt: formatter.toKm }}
              />
              <RowStat
                label={`Total Time`}
                value={timeConverter.fromSeconds(totalMinutes)}
                addons={{ IconSource: IconClock, formatt: formatter.toDuration }}
              />
            </Stack>
            <RegionTripsTimeline trips={trips} />
          </Stack>
        );
      }}
    </FetchWrapper>
  );
}
