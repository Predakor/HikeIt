import { IconCalendar, IconPeaks } from "@/Icons/Icons";
import type { BaseTrip, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { PartialMap } from "@/types/Utils/MappingTypes";
import type { StatAddons } from "@/types/Utils/StatTypes";
import type { IconType } from "react-icons";

export const icons: PartialMap<BaseTrip, StatAddons> = {
  // distance: { IconSource: GiJourney, unit: "m" },
  // duration: { IconSource: LuTimer, unit: "min" },
  // height: { IconSource: FaArrowTrendUp, unit: "m" },
  tripDay: { IconSource: IconCalendar },
};

export const menuIcons: PartialMap<TripDtoFull, IconType> = {
  reachedPeaks: IconPeaks,
};
