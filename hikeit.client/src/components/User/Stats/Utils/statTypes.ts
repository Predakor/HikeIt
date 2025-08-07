export type UserStats = {
  totals: Totals;
  locations: Locations;
  metas: Metas;
};

export type Totals = {
  totalDistanceMeters: number;
  totalAscentMeters: number;
  totalDescentMeters: number;
  totalDuration: string; // ISO duration or you can convert to number (seconds) if needed
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
