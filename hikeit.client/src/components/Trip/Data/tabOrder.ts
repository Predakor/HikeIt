import type { BasicAnalytics } from "@/types/Api/analytics.types";
import type { TabConfig } from "@/types/Utils/order.types";
import TripGrap from "../Details/TripGraph/TripGrap";
import { RouteAnalytics, TimeAnalytics, PeaksAnalytics } from "../Details";

export const tripAnalyticTabs: TabConfig<BasicAnalytics> = [
  { key: "route", label: "", Component: RouteAnalytics },
  { key: "time", label: "", Component: TimeAnalytics },
  { key: "peaks", label: "", Component: PeaksAnalytics },
  { key: "elevation", label: "", Component: TripGrap },
];
