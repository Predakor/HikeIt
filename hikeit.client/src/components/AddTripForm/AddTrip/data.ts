import type { CreateTrip } from "@/types/ApiTypes/TripDtos";
import type { StatAddons } from "@/types/Utils/StatTypes";

export type CreateTripConfigEntry<T> = {
  name: keyof T;
  type: React.HTMLInputTypeAttribute;
  label: string;
  unitAddods?: StatAddons;
};

export const tripFormConfig: CreateTripConfigEntry<CreateTrip["base"]>[] = [
  {
    name: "name",
    label: "trip name",
    type: "string",
  },
  { name: "tripDay", label: "trip day", type: "date" },
];
