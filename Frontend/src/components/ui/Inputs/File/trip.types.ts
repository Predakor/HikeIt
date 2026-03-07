export interface GraphData {
  ele: number;
  lat: number;
  lon: number;
}
[];

export interface ReachedPeak {
  name: string;
  height: number;
  reachedAt?: string;
  firstTime: boolean;
}
export interface ReachedPeakWithBadges extends ReachedPeak {
  isHighest: boolean;
}
