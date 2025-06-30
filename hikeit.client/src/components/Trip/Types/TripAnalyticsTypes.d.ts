import type { ReachedPeak } from "@/components/AddTripForm/AddFile/tripTypes";

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

export interface PeaksAnalyticData {
  summary: PeakSummaryData;
  reached: ReachedPeak[];
}

export interface PeakSummaryData {
  total: number;
  unique: number;
  highest: ReachedPeak;
  firstTimeVisits: number;
}
