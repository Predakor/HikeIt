import {
  UserProfilePage,
  UserSettingsPage,
  UserSummaryPage,
} from "@/pages/__index";
import type { RouteGroup } from "./routeTypes";

export const userRoutes: RouteGroup = {
  type: "group",
  path: "user",
  label: "User",
  pages: [
    {
      type: "item",
      path: "profile",
      label: "Profile",
      Page: UserProfilePage,
    },
    {
      type: "item",
      path: "options",
      label: "Options",
      Page: UserSettingsPage,
    },
    {
      type: "item",
      path: "summary",
      label: "Summary",
      Page: UserSummaryPage,
    },
  ],
};
