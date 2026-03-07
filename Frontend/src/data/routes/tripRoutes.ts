import { TripsPage, AddTripPage, TripDetailsPage } from "@/pages";
import type { RouteGroup } from "./route.types";

export const tripRoutes: RouteGroup = {
  type: "group",
  path: "trips",
  label: "Trips",
  pages: [
    {
      type: "item",
      path: "",
      label: "Trips",
      Page: TripsPage,
      // Icon: FaRoute,
    },
    {
      type: "item",
      path: "add",
      label: "Add new tirp",
      hidden: true,
      Page: AddTripPage,
      // Icon: BadgeIcon,
    },
    {
      type: "item",
      path: ":tripId",
      label: "trip details",
      hidden: true,
      Page: TripDetailsPage,
    },
  ],
};
