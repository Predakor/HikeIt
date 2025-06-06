import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import type { TripAnalytic } from "@/components/AddTripForm/AddTrip/tripTypes";
import RowStat from "@/components/Stats/RowStat";
import { For, Stack } from "@chakra-ui/react";
import { analyticIcons } from "../../Data/ValueMap";

function TripAnalytics({ data }: { data: TripAnalytic }) {
  if (!data) {
    console.log("chuju co robisz");
  }
  const stats = ObjectToArray(data);

  return (
    <Stack gap={4}>
      <For
        each={stats}
        children={([name, value]) => (
          <RowStat
            value={GenericFormatter(value)}
            addons={analyticIcons[name]}
            label={name}
            key={name}
          />
        )}
      />
    </Stack>
  );
}

export default TripAnalytics;
