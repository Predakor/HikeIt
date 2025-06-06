import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import type { BaseTrip } from "@/components/AddTripForm/AddTrip/tripTypes";
import RowStat from "@/components/Stats/RowStat";
import { For } from "@chakra-ui/react";
import { icons } from "../../Data/ValueMap";

function TripBase({ data }: { data: BaseTrip }) {
  const stats = ObjectToArray(data);

  return (
    <For
      each={stats}
      children={([name, value]) => {
        return (
          <RowStat
            value={GenericFormatter(value)}
            addons={icons[name]}
            label={name}
            key={name}
          />
        );
      }}
    />
  );
}

export default TripBase;
