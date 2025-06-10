import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import RowStat from "@/components/Stats/RowStat";
import { For } from "@chakra-ui/react";
import type { BaseTrip } from "@/types/ApiTypes/TripDtos";
import { icons } from "../../Data/statsInfo";

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
