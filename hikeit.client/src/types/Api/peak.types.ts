export interface Peak {
  id: number;
  name: string;
  height: number;
}
export interface PeakWithLocation extends Peak {
  lat: number;
  lon: number;
}

export interface PeakWithReachStatus extends Peak {
  reached: boolean;
}
