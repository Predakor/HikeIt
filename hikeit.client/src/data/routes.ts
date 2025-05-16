import LandingPage from "@pages/LandingPage";
import TripsPage from "@pages/TripsPage";
import RegionsPage from "@pages/RegionsPage";
import BadgesPage from "@pages/BadgesPage";
import type { FunctionComponent, ReactNode } from "react";

interface RouteData {
  path: string;
  label: string;
  Page: FunctionComponent;
  Icon?: ReactNode; // optional icon, e.g. from react-icons or chakra icons
}

export const routes: RouteData[] = [
  {
    path: "/",
    label: "Home",
    Page: LandingPage,
    // Icon: HomeIcon, // example icon placeholder
  },
  {
    path: "/trips",
    label: "Trips",
    Page: TripsPage,
    // Icon: TravelIcon,
  },
  {
    path: "/regions",
    label: "Regions",
    Page: RegionsPage,
    // Icon: MapIcon,
  },
  {
    path: "/badges",
    label: "Badges",
    Page: BadgesPage,
    // Icon: BadgeIcon,
  },
];
