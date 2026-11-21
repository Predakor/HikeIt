import type { ReachedPeak } from "@/components/ui/Inputs/File/trip.types";
import type { TimeSpanString } from "./types";

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
  visualisation: {};
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

  // Null when the metric is off, the track is shorter than its window, or the trip predates it.
  steepestClimbPercent?: number | null;
  steepestDescentPercent?: number | null;

  // Null when the metric is off, the track has no such segment, or the trip predates it.
  longestClimbDistanceM?: number | null;
  longestClimbPercent?: number | null;
  longestDescentDistanceM?: number | null;
  longestDescentPercent?: number | null;
}

export interface TimeAnalytic {
  duration: TimeSpanString; // actually iso stirng
  startTime: TimeSpanString; //actually iso stirng
  endTime: TimeSpanString; //actually iso stirng

  activeTime: TimeSpanString;
  idleTime: TimeSpanString;
  ascentTime: TimeSpanString;
  descentTime: TimeSpanString;

  averageSpeedKph: number;
  averageAscentKph: number;
  averageDescentKph: number;

  // Null when the speed segment metric is off, the track is shorter than its window, or the trip predates it.
  fastestSpeedKph?: number | null;
  slowestSpeedKph?: number | null;
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
