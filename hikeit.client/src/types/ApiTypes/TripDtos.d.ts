import type {
  RouteAnalytic,
  TimeAnalytics,
} from "@/components/Trip/Types/TripAnalyticsTypes";

export interface BaseTrip {
  name: string;
  tripDay: string;
}

export interface TripDto {
  id: string;
  base: BaseTrip;
  regionId?: number;
}

interface TripAnalytic {
  routeAnalytics: RouteAnalytic;
  timeAnalytics?: TimeAnalytics;
  peaksAnalytics?: PeaksAnalytic;
  elevationProfile?: {};
}

export interface TripDtoFull extends TripDto {
  region?: {
    id: number;
    name: string;
  };
  trackAnalytic: TripAnalytic | null;
  trackGraph?: GraphData[];
  reachedPeaks?: ReachedPeak[];
  gpxFile?: {};
}

export interface CreateTrip {
  regionId: number;
  base: BaseTrip;
  file?: File;
}

export interface FieldEntry {
  name: string;
  type: HTMLInputTypeAttribute;
  label: string;
  unit?: string;
}

export type Gains = {
  plannarDist: number; //distance between this and previous point in 2D
  eleDelta: number; //elevation difference betwen current and last point
  slope: number; //% of slope between this and prev point
};

export type GpxEntry = {
  lat: number;
  lon: number;
  ele: number;
  time?: string;
  gains?: Gains;
};

export type GpxArray = Array<GpxEntry>;
