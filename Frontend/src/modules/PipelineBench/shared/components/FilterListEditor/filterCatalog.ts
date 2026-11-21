import type {
  PaceFilterConfig,
  PeakFilterConfig,
  PointFilterConfig,
  TerrainFilterConfig,
} from "@/modules/PipelineBench/shared/types/FilterConfig";
import type { GradeTargetsField } from "./Inputs/GradeTargetsInput";
import type { NumberField } from "./Inputs/NumberInput";
import type { BooleanField } from "./Inputs/SwitchInput";
import type { PointFieldsField } from "./Inputs/TargetsInput";

export type FilterKind = "point" | "peak" | "pace" | "terrain";
export type FilterItem = PointFilterConfig | PeakFilterConfig | PaceFilterConfig | TerrainFilterConfig;

export type FilterField = NumberField | BooleanField | PointFieldsField | GradeTargetsField;
export type FilterFieldValue = number | boolean | string;

export type FilterFields = readonly FilterField[];

const targetsField: PointFieldsField = {
  kind: "pointFields",
  name: "targets",
  defaultValue: "Elevation",
};

export const pointFilterCatalog: Record<PointFilterConfig["filterType"], FilterFields> = {
  MedianSmoothingFilter: [
    {
      kind: "number",
      name: "windowSize",
      control: "range",
      min: 2,
      max: 20,
      step: 1,
      defaultValue: 6,
    },
    targetsField,
  ],
  EmaSmoothingFilter: [
    {
      kind: "number",
      name: "alpha",
      control: "range",
      min: 0.01,
      max: 1,
      step: 0.01,
      defaultValue: 0.88,
    },
    targetsField,
  ],
  MaxSpikeFilter: [
    {
      kind: "number",
      name: "maxSpike",
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
      name: "decimals",
      control: "range",
      min: 0,
      max: 6,
      step: 1,
      defaultValue: 1,
    },
    targetsField,
  ],
  PinLocalMaximaFilter: [{ kind: "boolean", name: "enabled", defaultValue: true }],
  SimplifyFilter: [
    {
      kind: "number",
      name: "toleranceM",
      control: "range",
      min: 0,
      max: 50,
      step: 0.5,
      defaultValue: 3,
    },
  ],
};

export const peakFilterCatalog: Record<PeakFilterConfig["filterType"], FilterFields> = {
  LocalMaximaFilter: [
    {
      kind: "number",
      name: "minClimbM",
      control: "number",
      min: 0,
      max: 500,
      step: 1,
      defaultValue: 0,
    },
  ],
  MergeNearbyByDistanceFilter: [
    {
      kind: "number",
      name: "distanceM",
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
      name: "elevationDeltaM",
      control: "number",
      min: 0,
      max: 500,
      step: 1,
      defaultValue: 50,
    },
  ],
  PeakLookupFilter: [
    {
      kind: "number",
      name: "offsetM",
      control: "number",
      min: 0,
      max: 5000,
      step: 10,
      defaultValue: 100,
    },
  ],
  MatchWithinDistanceFilter: [
    {
      kind: "number",
      name: "maxDistanceM",
      control: "number",
      min: 1,
      max: 5000,
      step: 10,
      defaultValue: 100,
    },
  ],
};

export const paceFilterCatalog: Record<PaceFilterConfig["filterType"], FilterFields> = {
  IdleFilter: [
    {
      kind: "number",
      name: "speedKph",
      control: "number",
      min: 0,
      max: 10,
      step: 0.1,
      defaultValue: 0.5,
    },
  ],
  GradeFilter: [
    {
      kind: "number",
      name: "minGradePercent",
      control: "number",
      min: 0,
      max: 50,
      step: 0.5,
      defaultValue: 2,
    },
    { kind: "gradeTargets", name: "targets", defaultValue: "Both" },
  ],
  SpeedSegmentFilter: [
    {
      kind: "number",
      name: "windowM",
      control: "number",
      min: 10,
      max: 5000,
      step: 10,
      defaultValue: 100,
    },
  ],
};

export const terrainFilterCatalog: Record<TerrainFilterConfig["filterType"], FilterFields> = {
  ElevationGainFilter: [
    {
      kind: "number",
      name: "minChangeM",
      control: "number",
      min: 0.5,
      max: 50,
      step: 0.5,
      defaultValue: 3,
    },
  ],
  LongestClimbFilter: [
    {
      kind: "number",
      name: "minChangeM",
      control: "number",
      min: 0.5,
      max: 50,
      step: 0.5,
      defaultValue: 3,
    },
  ],
  SteepestSegmentFilter: [
    {
      kind: "number",
      name: "windowM",
      control: "number",
      min: 10,
      max: 5000,
      step: 10,
      defaultValue: 100,
    },
  ],
};

const filterCatalogByKind: Record<FilterKind, Record<string, FilterFields>> = {
  point: pointFilterCatalog,
  peak: peakFilterCatalog,
  pace: paceFilterCatalog,
  terrain: terrainFilterCatalog,
};

// Keys are camelCase on the wire; the editor shows them capitalised.
export function fieldLabel(name: string): string {
  return name[0].toUpperCase() + name.slice(1);
}

export function filterTypesOf(kind: FilterKind): string[] {
  return Object.keys(filterCatalogByKind[kind]);
}

export function fieldsOf(kind: FilterKind, filterType: string): FilterFields {
  return filterCatalogByKind[kind][filterType];
}

export function createFilterItem(filterType: string, fields: FilterFields): FilterItem {
  const values = Object.fromEntries(fields.map((field) => [field.name, field.defaultValue]));

  return { filterType: filterType, ...values } as FilterItem;
}

// Replaces one field and keeps the filter's other fields (e.g. targets next to alpha).
export function withFieldValue(
  filter: FilterItem,
  name: string,
  value: FilterFieldValue,
): FilterItem {
  return { ...filter, [name]: value } as FilterItem;
}

export function readFieldValue(filter: FilterItem, name: string): FilterFieldValue | undefined {
  const values: Record<string, unknown> = { ...filter };

  return values[name] as FilterFieldValue | undefined;
}

// A grade rule ahead of the idle rule claims standing segments on a slope as climbing. Warning only.
export function hasGradeBeforeIdle(filters: FilterItem[]): boolean {
  const gradeIndex = filters.findIndex((filter) => filter.filterType === "GradeFilter");

  if (gradeIndex === -1) {
    return false;
  }

  return filters.slice(gradeIndex + 1).some((filter) => filter.filterType === "IdleFilter");
}

// Simplify before PinLocalMaxima drops the summits the pin was meant to keep. Warning only, the list is not blocked.
export function hasSimplifyBeforePinLocalMaxima(filters: FilterItem[]): boolean {
  const simplifyIndex = filters.findIndex((filter) => filter.filterType === "SimplifyFilter");

  if (simplifyIndex === -1) {
    return false;
  }

  return filters
    .slice(simplifyIndex + 1)
    .some((filter) => filter.filterType === "PinLocalMaximaFilter");
}
