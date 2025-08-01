import type { TripAnalytic, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { TabConfig } from "@/types/Utils/OrderTypes";
import { ELevationGraph } from "../TripDetails/lazy";
import { RouteAnalytics, TimeAnalytics, PeaksAnalytics } from "../TripDetails";

export const tripAnalyticTabs: TabConfig<TripAnalytic> = [
  { key: "routeAnalytics", label: "", Component: RouteAnalytics },
  { key: "timeAnalytics", label: "", Component: TimeAnalytics },
  { key: "peakAnalytics", label: "", Component: PeaksAnalytics },
  { key: "elevationProfile", label: "", Component: ELevationGraph },
];

export const tripDetailsTabs: TabConfig<TripDtoFull> = [
  {
    type: "group",
    label: "Analytics",
    base: "trackAnalytic",
    items: tripAnalyticTabs,
  },
  { key: "region", label: "Region", Component: ELevationGraph },
  // { key: "pictures", label: "pictures", Component: TripGraph },
] as const;
