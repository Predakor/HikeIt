import {
  BadgesPage,
  LandingPage,
  RegionsSummaries,
  TripsPage,
  UserStatsPage,
} from "@/pages";
import type { RouteItem } from "./route.types";

export const navRoutes: RouteItem[] = [
  {
    type: "item",
    path: "/",
    label: "Home",
    hidden: true,
    Page: LandingPage,
    // Icon: HomeIcon, // example icon placeholder
  },
  {
    type: "item",
    path: "trips",
    label: "Trips",
    Page: TripsPage,
    // Icon: FaRoute,
  },
  {
    type: "item",
    path: "regions",
    label: "Regions",
    Page: RegionsSummaries,
    // Icon: MapIcon,
  },
  {
    type: "item",
    path: "stats",
    label: "Stats",
    Page: UserStatsPage,
    // Icon: MapIcon,
  },
  {
    type: "item",
    path: "badges",
    label: "Badges",
    Page: BadgesPage,
    // Icon: BadgeIcon,
  },
];
