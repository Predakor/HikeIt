export interface Trip {
  id: number;
  height: number; // Required float
  length: number; // Required float
  duration: number; // Required float
  tripDay?: string; // DateOnly will be represented as a string in TypeScript (ISO format)

  regionID: number; // Required int
}

export type ResourceUrl = string;
