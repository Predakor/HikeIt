import { adminRoutes } from "./admin/adminRoutes";
import { authRoutes } from "./authRoutes";
import { navRoutes } from "./navRoutes";
import { regionRoutes } from "./regionRoutes";
import type { LazyPage, RouteEntry } from "./route.types";
import { tripRoutes } from "./tripRoutes";
import { userRoutes } from "./userRoutes";

export const routes: RouteEntry[] = [
  ...navRoutes,
  authRoutes,
  tripRoutes,
  userRoutes,
  regionRoutes,
  adminRoutes,
];

export const preload = (path: string) => {
  const page = pages.get(path);
  if (page) {
    page.preload();
  }
};

const pages: Map<string, LazyPage> = new Map();

routes.forEach((route) => {
  if (route.type === "item") {
    pages.set(route.path, route.Page);
    return;
  }

  route.pages.forEach((page) => {
    pages.set(route.path + "/" + page.path, page.Page);
  });
});
