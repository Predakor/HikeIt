export interface BaseTrip {
  height: number;
  distance: number;
  duration: number;
  tripDay: string;
}

export interface GraphData {
  ele: number;
  lat: number;
  lon: number;
}
[];

export interface ReachedPeak {}

export interface TripAnalytic {
  totalDistanceKm: number;
  totalAscent: number;
  totalDescent: number;
  minElevation: number;
  maxElevation: number;

  //Owned Types
  TripTimeAnalytic?: TripTimeAnalytics;
  ReachedPeaks?: ReachedPeak[];
}

export interface TripTimeAnalytics {
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
