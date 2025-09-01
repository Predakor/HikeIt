import type { TimeString } from "@/Utils/Formatters/Duration/Duration";

export type UserStats = {
  totals: Totals;
  locations: Locations;
  metas: Metas;
};

export type Totals = {
  totalDistanceMeters: number;
  totalAscentMeters: number;
  totalDescentMeters: number;
  totalDuration: TimeString;
  totalClimbTime: TimeString;
  totalDescentTime: TimeString;
  totalPeaks: number;
  totalTrips: number;
};

export type Locations = {
  uniquePeaks: number;
  regionsVisited: number;
};

export type Metas = {
  firstHikeDate: string | null; // ISO date string e.g. '2025-01-01'
  lastHikeDate: string | null;
  longestTripMeters: number;
};
