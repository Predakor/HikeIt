import LoginPage from "@/pages/Auth/LoginPage";
import RegisterPage from "@/pages/Auth/RegisterPage";
import AddTripPage from "@/pages/Trips/AddTripPage";
import TripDetailsPage from "@/pages/Trips/TripDetailsPage";
import TripsPage from "@/pages/Trips/TripsPage";
import BadgesPage from "@pages/BadgesPage";
import LandingPage from "@pages/LandingPage";
import RegionsPage from "@pages/RegionsPage";
import type { FunctionComponent, ReactNode } from "react";

interface RouteData {
  path: string;
  label: string;
  Page: FunctionComponent;
  hidden?: boolean;
  Icon?: ReactNode; // optional icon, e.g. from react-icons or chakra icons
}

export const routes: RouteData[] = [
  { path: "/auth/login", label: "Auth", hidden: true, Page: LoginPage },
  { path: "/auth/register", label: "Auth", hidden: true, Page: RegisterPage },

  {
    path: "/",
    label: "Home",
    hidden: true,
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
    path: "/trips/add",
    label: "Add new tirp",
    hidden: true,
    Page: AddTripPage,
    // Icon: BadgeIcon,
  },
  {
    path: "/trips/:tripId",
    label: "trip details",
    hidden: true,
    Page: TripDetailsPage,
    // Icon: BadgeIcon,
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
