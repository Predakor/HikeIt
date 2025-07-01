import type { RouteEntry } from "./routeTypes";
import { authRoutes } from "./authRoutes";
import { tripRoutes } from "./tripRoutes";
import { navRoutes } from "./navRoutes";
import { userRoutes } from "./userRoutes";

export const routes: RouteEntry[] = [
  ...navRoutes,
  authRoutes,
  tripRoutes,
  userRoutes,
];
