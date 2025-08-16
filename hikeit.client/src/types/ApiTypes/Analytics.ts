import type { ReachedPeak } from "@/components/AddFile/AddFile/tripTypes";

export interface TripAnalytic {
  routeAnalytics: RouteAnalytic;
  timeAnalytics?: TimeAnalytic;
  peakAnalytics?: PeaksAnalytics;
  elevationProfile?: {};
}

export interface BasicAnalytics {
  route: RouteAnalytic;
  time?: TimeAnalytic;
  peaks: string | null;
  elevation: string | null;
}

export interface RouteAnalytic {
  totalDistanceKm: number;
  totalAscent: number;
  totalDescent: number;

  highestElevation: number;
  lowestElevation: number;

  averageSlope: string;
  averageAscentSlope: string;
  averageDescentSlope: string;
}

export interface TimeAnalytic {
  duration: number; // in milliseconds
  startTime: Date;
  endTime: Date;

  activeTime: number;
  idleTime: number;
  ascentTime: number;
  descentTime: number;

  averageSpeedKph: number;
  averageAscentKph: number;
  averageDescentKph: number;
}

export interface PeaksAnalytics {
  summary: PeakSummaryData;
  reached: ReachedPeak[];
}

export interface PeakSummaryData {
  total: number;
  unique: number;
  new: number;
  highest: ReachedPeak;
}
