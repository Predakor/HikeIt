import type { TripAnalytic, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { TabConfig } from "@/types/Utils/OrderTypes";
import RouteAnalytics from "../TripDetails/RouteAnalytics/RouteAnalytics";
import TripGraph from "../TripDetails/TripGraph/TripGraph";
import PreviewGraphDevOnly from "../TripDetails/TripGraph/Dev_PreviewGraph/PreviewGraphDevOnly";
import TimeAnalytic from "../TripDetails/TimeAnalytics/TimeAnalytics";
import PeaksAnalytics from "../TripDetails/PeaksAnalytics/PeaksAnalytics";

export const tripAnalyticTabs: TabConfig<TripAnalytic> = [
  { key: "routeAnalytics", label: "", Component: RouteAnalytics },
  { key: "timeAnalytics", label: "", Component: TimeAnalytic },
  { key: "peakAnalytics", label: "", Component: PeaksAnalytics },
  { key: "elevationProfile", label: "", Component: PreviewGraphDevOnly },
];

export const tripDetailsTabs: TabConfig<TripDtoFull> = [
  { key: "base", label: "Base" },
  {
    type: "group",
    label: "Analytics",
    base: "trackAnalytic",
    items: tripAnalyticTabs,
  },
  { key: "region", label: "Region", Component: TripGraph },
  { key: "pictures", label: "pictures", Component: TripGraph },
] as const;
