import type {
  GraphData,
  ReachedPeak,
} from "@/components/AddFile/AddFile/tripTypes";
import type { HTMLInputTypeAttribute } from "react";
import type {
  BasicAnalytics,
  RouteAnalytic,
  TimeAnalytic,
  TripAnalytic,
} from "./Analytics";

export interface TripDto {
  id: string;
  base: {
    name: string;
    tripDay: string;
  };
  region: {
    id: number;
    name: string;
  };
}

export interface TripSummary {
  id: string;
  name: string;
  tripDay: string;
  region: {
    id: number;
    name: string;
  };
}

export type TripSummaries = TripSummary[];

export interface TripWithBasicAnalytics {
  id: string;
  name: string;
  tripDay: string;
  analytics: BasicAnalytics;
}

export interface TripDtoFull extends TripDto {
  trackAnalytic: TripAnalytic | null;
  trackGraph?: GraphData[];
  reachedPeaks?: ReachedPeak[];
  gpxFile?: {};
}

export interface FieldEntry {
  name: string;
  type: HTMLInputTypeAttribute;
  label: string;
  unit?: string;
}
