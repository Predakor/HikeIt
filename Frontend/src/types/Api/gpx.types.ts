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
