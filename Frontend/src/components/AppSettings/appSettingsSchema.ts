import type { InputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";

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
  type: number;
}

export const SchemaInputMap: Record<PropertyType, InputConfigEntry["type"]> = {
  string: "text",
  number: "number",
  integer: "number",
  boolean: "checkbox",
};
