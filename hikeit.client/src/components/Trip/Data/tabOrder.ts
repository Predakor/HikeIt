import type { TripAnalytic, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { TabConfig } from "@/types/Utils/OrderTypes";
import RouteAnalytics from "../TripDetails/RouteAnalytics/RouteAnalytics";
import TripBase from "../TripDetails/TripBase/TripBase";
import TripGraph from "../TripDetails/TripGraph/TripGraph";

export const tripAnalyticTabs: TabConfig<TripAnalytic> = [
  { key: "routeAnalytics", label: "Time", Component: RouteAnalytics },
  { key: "timeAnalytics", label: "Time", Component: RouteAnalytics },
  { key: "elevationProfile", label: "Time", Component: TripGraph },
  { key: "peaksAnalytics", label: "Time", Component: RouteAnalytics },
];

export const tripDetailsTabs: TabConfig<TripDtoFull> = [
  { key: "base", label: "Base", Component: TripBase },
  {
    key: "trackAnalytic",
    label: "Analytics",
    Component: RouteAnalytics,
  },
  { key: "region", label: "Region", Component: TripGraph },
  { key: "pictures", label: "pictures", Component: TripGraph },
];
