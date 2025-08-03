import type {
  GraphData,
  ReachedPeak,
} from "@/components/AddFile/AddFile/tripTypes";
import type { HTMLInputTypeAttribute } from "react";
import type { TripAnalytic } from "./Analytics";

export interface TripDto {
  id: string;
  name: string;
  tripDay: string;
  region: {
    id: number;
    name: string;
  };
}

export interface TripDtoFull extends TripDto {
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
