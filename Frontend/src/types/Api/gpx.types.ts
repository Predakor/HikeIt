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
};

export type GpxEntryWithTime = GpxEntry & {
  time?: string;
};

type GpxEntryWithGains = GpxEntry & {
  time?: string;
  gains: Gain;
};

export type GpxArray = Array<GpxEntryWithTime>;
export type GpxArrayWithGains = GpxEntryWithGains[];
