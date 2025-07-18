import type {
  RouteAnalytic,
  TimeAnalytic,
} from "@/components/Trip/Types/TripAnalyticsTypes";

export interface TripDto {
  id: string;
  name: string;
  tripDay: string;
  region: {
    id: number;
    name: string;
  };
}

interface TripAnalytic {
  routeAnalytics: RouteAnalytic;
  timeAnalytics?: TimeAnalytic;
  peakAnalytics?: PeaksAnalytic;
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

export interface Gain {
  plannarDist: number; //distance between this and previous point in 2D
  eleDelta: number; //elevation difference betwen current and last point
  slope: number; //% of slope between this and prev point
}

export type Gains = Gain[];

export type GpxEntry = {
  lat: number;
  lon: number;
  ele: number;
  time?: string;
};

type GpxEntryWithGains = {
  lat: number;
  lon: number;
  ele: number;
  time?: string;
  gains: Gain;
};

export type GpxArray = Array<GpxEntry>;
export type GpxArrayWithGains = GpxEntryWithGains[];
