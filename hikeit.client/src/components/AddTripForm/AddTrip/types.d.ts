interface TripDto {
  height: number;
  distance: number;
  duration: number;
  regionId: number;
  tripDay: string;
}

interface FieldEntry {
  name: string;
  type: HTMLInputTypeAttribute;
  label: string;
  unit?: string;
}
