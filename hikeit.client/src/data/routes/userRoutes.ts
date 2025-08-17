import { UserProfilePage, UserSettingsPage } from "@/pages";
import type { RouteGroup } from "./route.types";

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
  ],
};
