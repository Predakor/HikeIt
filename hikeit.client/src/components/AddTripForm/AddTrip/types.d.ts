export interface TripDto {
  height: number;
  distance: number;
  duration: number;
  regionId: number;
  tripDay: string;
}

export interface FieldEntry {
  name: string;
  type: HTMLInputTypeAttribute;
  label: string;
  unit?: string;
}

export type Gains = {
  plannarDist: number; //distance between this and previous point in 2D
  eleDelta: number; //elevation difference betwen current and last point
  slope: number; //% of slope between this and prev point
};

export type GpxEntry = {
  lat: number;
  lon: number;
  ele: number;
  time?: string;
  gains?: Gains;
};

export type GpxArray = Array<GpxEntry>;
