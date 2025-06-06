import type { TabEntry } from "@/types/TabTypes";
import TripAnalytics from "../TripDetails/TripAnalytics/TripAnalytics";
import TripBase from "../TripDetails/TripBase/TripBase";
import TripGraph from "../TripDetails/TripGraph/TripGraph";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";

export const tripDetailsTabs: TabEntry<TripDtoFull>[] = [
  { key: "base", label: "Base", Component: TripBase },
  { key: "trackAnalytic", label: "Analytics", Component: TripAnalytics },
  { key: "trackGraph", label: "Graph", Component: TripGraph },
  { key: "reachedPeaks", label: "Reached Peaks", Component: TripGraph },
  { key: "region", label: "Region", Component: TripGraph },
];
