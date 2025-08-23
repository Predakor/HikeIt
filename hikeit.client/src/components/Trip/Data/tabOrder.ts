import type { BasicAnalytics } from "@/types/ApiTypes/Analytics";
import type { TabConfig } from "@/types/Utils/order.types";
import { RouteAnalytics, TimeAnalytics, PeaksAnalytics } from "../Details";
import TripGrap from "../Details/TripGraph/TripGrap";

export const tripAnalyticTabs: TabConfig<BasicAnalytics> = [
  { key: "route", label: "", Component: RouteAnalytics },
  { key: "time", label: "", Component: TimeAnalytics },
  { key: "peaks", label: "", Component: PeaksAnalytics },
  { key: "elevation", label: "", Component: TripGrap },
];
