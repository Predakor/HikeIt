export interface Trip {
  id: number;
  height: number; // Required float
  length: number; // Required float
  duration: number; // Required float
  tripDay?: string; // DateOnly will be represented as a string in TypeScript (ISO format)

  regionID: number; // Required int
}

export interface Region {
  id: number;
  name: string;
}

export interface RegionProgressSummary {
  region: Region;
  uniqueReachedPeaks: number;
  totalPeaksInRegion: number;
}

export interface RegionProgressFull {}
