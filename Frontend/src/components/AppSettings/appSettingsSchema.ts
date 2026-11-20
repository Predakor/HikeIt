import type { InputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";

// The names of the server's AppSettingType enum, which it sends as strings.
export const AppSettingType = {
  GpxDataProcessing: "GpxDataProcessing",
  PeakMatching: "PeakMatching",
  ElevationProfile: "ElevationProfile",
  Route: "Route",
} as const;

export type AppSettingTypeId = (typeof AppSettingType)[keyof typeof AppSettingType];

type PropertyType = "number" | "string" | "integer" | "boolean";

export interface AppSettingsEntry<TValue extends object = Record<string, unknown>> {
  id: number;
  name: string;
  value: TValue;
  schema: {
    type: PropertyType;
    properties: Record<keyof TValue, { type: PropertyType }>;
    required: (keyof TValue)[];
  };
  type: string;
}

export const SchemaInputMap: Record<PropertyType, InputConfigEntry["type"]> = {
  string: "text",
  number: "number",
  integer: "number",
  boolean: "checkbox",
};
