import type { PeakFilterConfig, PointFilterConfig } from "@/types/Api/pipelinePreview.types";

export type FilterKind = "point" | "peak";
export type FilterItem = PointFilterConfig | PeakFilterConfig;

// One editable value per filter, named after the field of the wire `value` object.
export interface NumberField {
  kind: "number";
  name: string;
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

export type FilterField = NumberField | BooleanField;

export const pointFilterCatalog: Record<PointFilterConfig["FilterType"], FilterField> = {
  MedianSmoothingFilter: { kind: "number", name: "WindowSize", min: 2, max: 20, step: 1, defaultValue: 6 },
  EmaSmoothingFilter: { kind: "number", name: "Alpha", min: 0.01, max: 1, step: 0.01, defaultValue: 0.88 },
  MaxSpikeFilter: { kind: "number", name: "MaxSpike", min: 1, max: 50, step: 0.5, defaultValue: 8 },
  RoundingPrecisionFilter: { kind: "number", name: "Decimals", min: 0, max: 5, step: 1, defaultValue: 1 },
  PinLocalMaximaFilter: { kind: "boolean", name: "Enabled", defaultValue: true },
  SimplifyFilter: { kind: "number", name: "ToleranceM", min: 0, max: 50, step: 0.5, defaultValue: 3 },
  CoordinateSmoothingFilter: { kind: "number", name: "Alpha", min: 0.01, max: 1, step: 0.01, defaultValue: 0.5 },
};

export const peakFilterCatalog: Record<PeakFilterConfig["FilterType"], FilterField> = {
  MergeNearbyByDistanceFilter: { kind: "number", name: "DistanceM", min: 0, max: 500, step: 1, defaultValue: 50 },
  MergeNearbyByElevationFilter: {
    kind: "number",
    name: "ElevationDeltaM",
    min: 0,
    max: 500,
    step: 1,
    defaultValue: 50,
  },
};

const filterCatalogByKind: Record<FilterKind, Record<string, FilterField>> = {
  point: pointFilterCatalog,
  peak: peakFilterCatalog,
};

export function filterTypesOf(kind: FilterKind): string[] {
  return Object.keys(filterCatalogByKind[kind]);
}

export function fieldOf(kind: FilterKind, filterType: string): FilterField {
  return filterCatalogByKind[kind][filterType];
}

export function createFilterItem(filterType: string, field: FilterField): FilterItem {
  return { FilterType: filterType, value: { [field.name]: field.defaultValue } } as FilterItem;
}

export function withFieldValue(filter: FilterItem, field: FilterField, value: number | boolean): FilterItem {
  return { FilterType: filter.FilterType, value: { [field.name]: value } } as FilterItem;
}

export function readFieldValue(filter: FilterItem): number | boolean | undefined {
  return Object.values(filter.value)[0];
}

// Simplify before PinLocalMaxima drops the summits the pin was meant to keep. Warning only, the list is not blocked.
export function hasSimplifyBeforePinLocalMaxima(filters: FilterItem[]): boolean {
  const simplifyIndex = filters.findIndex((filter) => filter.FilterType === "SimplifyFilter");

  if (simplifyIndex === -1) {
    return false;
  }

  return filters.slice(simplifyIndex + 1).some((filter) => filter.FilterType === "PinLocalMaximaFilter");
}
