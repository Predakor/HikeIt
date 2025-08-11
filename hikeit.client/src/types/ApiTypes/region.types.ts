import type { Peak, PeakWithLocation, PeakWithReachStatus } from "./peak.types";

export interface Region {
  id: number;
  name: string;
}

export interface RegionWithPeaks {
  region: Region;
  peaks: Peak[];
}

export interface RegionWithDetailedPeaks {
  region: Region;
  peaks: PeakWithLocation[];
}

export interface RegionProgressSummary {
  region: Region;
  uniqueReachedPeaks: number;
  totalPeaksInRegion: number;
}

export interface RegionProgressFull {
  region: Region;
  totalPeaksInRegion: number;
  totalReachedPeaks: number;
  uniqueReachedPeaks: number;
  highestPeak: Peak;
  peaks: PeakWithReachStatus[];
}
