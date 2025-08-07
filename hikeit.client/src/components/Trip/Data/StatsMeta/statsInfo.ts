import { IconCalendar, IconPeak } from "@/Icons/Icons";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { Trip } from "@/types/ApiTypes/types";
import type { PartialMap } from "@/types/Utils/MappingTypes";
import type { StatAddons } from "@/types/Utils/StatTypes";
import type { IconType } from "react-icons";

export const icons: PartialMap<Trip, StatAddons> = {
  // distance: { IconSource: GiJourney, unit: "m" },
  // duration: { IconSource: LuTimer, unit: "min" },
  // height: { IconSource: FaArrowTrendUp, unit: "m" },
  tripDay: { IconSource: IconCalendar },
};

export const menuIcons: PartialMap<TripDtoFull, IconType> = {
  reachedPeaks: IconPeak,
};
