import { LoginPage, RegisterPage } from "@/pages/__index";
import type { RouteGroup } from "./routeTypes";

export const authRoutes: RouteGroup = {
  type: "group",
  path: "auth",
  label: "Auth",
  hidden: true,
  pages: [
    {
      type: "item",
      path: "register",
      label: "Register",
      hidden: true,
      Page: RegisterPage,
    },
    {
      type: "item",
      path: "login",
      label: "Login",
      hidden: true,
      Page: LoginPage,
    },
  ],
};
