import type { LazyPage } from "@/data/routes/route.types";
import { lazy } from "react";

export const LoginPage = lazyWithPreload(
  () => import("@/pages/Auth/LoginPage")
);
export const RegisterPage = lazyWithPreload(
  () => import("@/pages/Auth/RegisterPage")
);

export const TripsPage = lazyWithPreload(
  () => import("@/pages/Trips/TripsPage")
);
export const AddTripPage = lazyWithPreload(
  () => import("@/pages/Trips/AddTripPage")
);
export const TripDetailsPage = lazyWithPreload(
  () => import("@/pages/Trips/TripDetailsPage")
);

export const BadgesPage = lazyWithPreload(() => import("@/pages/BadgesPage"));
export const LandingPage = lazyWithPreload(() => import("@/pages/LandingPage"));

export const RegionsSummaries = lazyWithPreload(
  () => import("@/pages/Regions/RegionSummariesPage")
);
export const RegionProgression = lazyWithPreload(
  () => import("@pages/Regions/RegionProgression")
);

export const UserProfilePage = lazyWithPreload(
  () => import("@pages/User/UserProfilePage")
);
export const UserSettingsPage = lazyWithPreload(
  () => import("@pages/User/UserSettingsPage")
);
export const UserStatsPage = lazyWithPreload(
  () => import("@/pages/User/UserStatsPage")
);

export function lazyWithPreload(factory: () => Promise<any>) {
  const Component = lazy(factory);
  (Component as any).preload = factory;
  return Component as LazyPage;
}
