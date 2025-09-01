import type { TimeString } from "@/Utils/Formatters/Duration/Duration";
import type { ReachedPeak } from "@/components/ui/Inputs/File/trip.types";

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
  totalDistanceMeters: number;
  totalAscentMeters: number;
  totalDescentMeters: number;

  highestElevationMeters: number;
  lowestElevationMeters: number;

  averageSlopePercent: number;
  averageAscentSlopePercent: number;
  averageDescentSlopePercent: number;
}

export interface TimeAnalytic {
  duration: TimeString; // actually iso stirng
  startTime: TimeString; //actually iso stirng
  endTime: TimeString; //actually iso stirng

  activeTime: TimeString;
  idleTime: TimeString;
  ascentTime: TimeString;
  descentTime: TimeString;

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
