import type { PeakFilterConfig, PointFilterConfig } from "@/types/Api/pipelinePreview.types";

export type FilterKind = "point" | "peak";
export type FilterItem = PointFilterConfig | PeakFilterConfig;

// Each field is one key of the wire `value` object, named after it.
// A number's `control` picks the widget: "range" slider for small bounded values, "number" input for wide ranges.
export interface NumberField {
  kind: "number";
  name: string;
  control: "range" | "number";
  min: number;
  max: number;
  step: number;
  defaultValue: number;
}

export interface BooleanField {
  kind: "boolean";
  name: string;
  defaultValue: boolean;
}

// Enum-like value, rendered as a select in the filter's row.
export interface SelectField {
  kind: "select";
  name: string;
  options: readonly string[];
  defaultValue: string;
}

export type FilterField = NumberField | BooleanField | SelectField;
export type FilterFieldValue = number | boolean | string;

// A filter's fields in row order. The wire value holds all of them; a filter may declare any number of extra fields.
export type FilterFields = readonly FilterField[];

export const pointFilterCatalog: Record<PointFilterConfig["FilterType"], FilterFields> = {
  MedianSmoothingFilter: [
    {
      kind: "number",
      name: "WindowSize",
      control: "range",
      min: 2,
      max: 20,
      step: 1,
      defaultValue: 6,
    },
  ],
  EmaSmoothingFilter: [
    {
      kind: "number",
      name: "Alpha",
      control: "range",
      min: 0.01,
      max: 1,
      step: 0.01,
      defaultValue: 0.88,
    },
    {
      kind: "select",
      name: "Targets",
      options: ["Elevation", "Coordinates", "Both"],
      defaultValue: "Elevation",
    },
  ],
  MaxSpikeFilter: [
    {
      kind: "number",
      name: "MaxSpike",
      control: "range",
      min: 1,
      max: 50,
      step: 0.5,
      defaultValue: 8,
    },
  ],
  RoundingPrecisionFilter: [
    {
      kind: "number",
      name: "Decimals",
      control: "range",
      min: 0,
      max: 5,
      step: 1,
      defaultValue: 1,
    },
  ],
  PinLocalMaximaFilter: [{ kind: "boolean", name: "Enabled", defaultValue: true }],
  SimplifyFilter: [
    {
      kind: "number",
      name: "ToleranceM",
      control: "range",
      min: 0,
      max: 50,
      step: 0.5,
      defaultValue: 3,
    },
  ],
};

export const peakFilterCatalog: Record<PeakFilterConfig["FilterType"], FilterFields> = {
  MergeNearbyByDistanceFilter: [
    {
      kind: "number",
      name: "DistanceM",
      control: "number",
      min: 0,
      max: 500,
      step: 1,
      defaultValue: 50,
    },
  ],
  MergeNearbyByElevationFilter: [
    {
      kind: "number",
      name: "ElevationDeltaM",
      control: "number",
      min: 0,
      max: 500,
      step: 1,
      defaultValue: 50,
    },
  ],
};

const filterCatalogByKind: Record<FilterKind, Record<string, FilterFields>> = {
  point: pointFilterCatalog,
  peak: peakFilterCatalog,
};

export function filterTypesOf(kind: FilterKind): string[] {
  return Object.keys(filterCatalogByKind[kind]);
}

export function fieldsOf(kind: FilterKind, filterType: string): FilterFields {
  return filterCatalogByKind[kind][filterType];
}

export function createFilterItem(filterType: string, fields: FilterFields): FilterItem {
  const value = Object.fromEntries(fields.map((field) => [field.name, field.defaultValue]));

  return { FilterType: filterType, value } as FilterItem;
}

// Replaces one field and keeps the filter's other fields (e.g. Targets next to Alpha).
export function withFieldValue(
  filter: FilterItem,
  name: string,
  value: FilterFieldValue,
): FilterItem {
  return { FilterType: filter.FilterType, value: { ...filter.value, [name]: value } } as FilterItem;
}

export function readFieldValue(filter: FilterItem, name: string): FilterFieldValue | undefined {
  const values: Record<string, unknown> = { ...filter.value };

  return values[name] as FilterFieldValue | undefined;
}

// Simplify before PinLocalMaxima drops the summits the pin was meant to keep. Warning only, the list is not blocked.
export function hasSimplifyBeforePinLocalMaxima(filters: FilterItem[]): boolean {
  const simplifyIndex = filters.findIndex((filter) => filter.FilterType === "SimplifyFilter");

  if (simplifyIndex === -1) {
    return false;
  }

  return filters
    .slice(simplifyIndex + 1)
    .some((filter) => filter.FilterType === "PinLocalMaximaFilter");
}
