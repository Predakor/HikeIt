// Mirrors the filter configs in Domain/Trips/Analytics/**/Filters/*.cs (the polymorphic filterType discriminator).

/** Server flags enum as comma-separated names ("Latitude, Elevation") or an alias ("Coordinates", "All"). Omitted means "Elevation". */
export type PointFields = string;

// Point filters work on track points (cleanup, elevation profile, route).
export type PointFilterConfig =
  | { filterType: "MedianSmoothingFilter"; windowSize: number; targets?: PointFields }
  | { filterType: "EmaSmoothingFilter"; alpha: number; targets?: PointFields }
  | { filterType: "MaxSpikeFilter"; maxSpike: number }
  | { filterType: "RoundingPrecisionFilter"; decimals: number; targets?: PointFields }
  | { filterType: "PinLocalMaximaFilter"; enabled: boolean }
  | { filterType: "SimplifyFilter"; toleranceM: number };

export type PeakFilterConfig =
  | { filterType: "LocalMaximaFilter"; minClimbM: number }
  | { filterType: "MergeNearbyByDistanceFilter"; distanceM: number }
  | { filterType: "MergeNearbyByElevationFilter"; elevationDeltaM: number }
  | { filterType: "PeakLookupFilter"; offsetM: number }
  | { filterType: "MatchWithinDistanceFilter"; maxDistanceM: number };

/** Flags enum as names: "Ascent", "Descent" or "Both". */
export type GradeTargets = string;

// Rules classify each time segment; the first rule that claims a segment wins.
export type PaceFilterConfig =
  | { filterType: "IdleFilter"; speedKph: number }
  | { filterType: "GradeFilter"; minGradePercent: number; targets?: GradeTargets }
  | { filterType: "SpeedSegmentFilter"; windowM: number };

// Plain metrics are always computed; a filter here replaces one (ElevationGain) or adds an optional metric.
export type TerrainFilterConfig =
  | { filterType: "ElevationGainFilter"; minChangeM?: number }
  | { filterType: "SteepestSegmentFilter"; windowM: number }
  | { filterType: "LongestClimbFilter"; minChangeM?: number };
