import LoginPage from "@/pages/Auth/LoginPage";
import RegisterPage from "@/pages/Auth/RegisterPage";
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
