import { lazy } from "react";

export const LoginPage = lazy(() => import("@/pages/Auth/LoginPage"));
export const RegisterPage = lazy(() => import("@/pages/Auth/RegisterPage"));

export const TripsPage = lazy(() => import("@/pages/Trips/TripsPage"));
export const AddTripPage = lazy(() => import("@/pages/Trips/AddTripPage"));
export const TripDetailsPage = lazy(
  () => import("@/pages/Trips/TripDetailsPage")
);

export const BadgesPage = lazy(() => import("@/pages/BadgesPage"));
export const LandingPage = lazy(() => import("@/pages/LandingPage"));

export const RegionsSummaries = lazy(
  () => import("@/pages/Regions/RegionSummaries")
);
export const RegionProgression = lazy(
  () => import("@pages/Regions/RegionProgression")
);

export const UserProfilePage = lazy(
  () => import("@pages/User/UserProfilePage")
);
export const UserSettingsPage = lazy(
  () => import("@pages/User/UserSettingsPage")
);
export const UserStatsPage = lazy(() => import("@/pages/User/UserStatsPage"));
