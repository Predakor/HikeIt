import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import { For } from "@chakra-ui/react";
import type { BaseTrip } from "../AddTripForm/AddTrip/tripTypes";
import RowStat from "../Stats/RowStat";
import { icons } from "./Data/ValueMap";

function Trip({ data }: { data: BaseTrip }) {
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

export default Trip;
