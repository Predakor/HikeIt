import {
  BadgesPage,
  LandingPage,
  RegionsPage,
  TripsPage,
} from "@/pages/__index";
import type { RouteItem } from "./routeTypes";

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
    path: "trips/",
    label: "Trips",
    Page: TripsPage,
    // Icon: FaRoute,
  },
  {
    type: "item",
    path: "/regions",
    label: "Regions",
    Page: RegionsPage,
    // Icon: MapIcon,
  },
  {
    type: "item",
    path: "/badges",
    label: "Badges",
    Page: BadgesPage,
    // Icon: BadgeIcon,
  },
];
