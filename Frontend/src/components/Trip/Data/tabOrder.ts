import type { BasicAnalytics } from "@/types/Api/analytics.types";
import type { TabConfig } from "@/types/Utils/order.types";
import { PeaksAnalytics, RouteAnalytics, TimeAnalytics } from "../Details";
import TripMap from "../Details/RouteVisualisation/TripVisualization";
import TripGrap from "../Details/TripGraph/TripGrap";

export const tripAnalyticTabs: TabConfig<BasicAnalytics> = [
  { key: "route", label: "", Component: RouteAnalytics },
  { key: "time", label: "", Component: TimeAnalytics },
  { key: "peaks", label: "", Component: PeaksAnalytics },
  { key: "elevation", label: "", Component: TripGrap },
  { key: "visualisation", label: "", Component: TripMap },
];
