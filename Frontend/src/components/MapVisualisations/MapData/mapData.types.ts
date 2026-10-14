import type { GpxEntry } from "@/types/Api/gpx.types";
import type { FlatPosition } from "@/Utils/Maps/routeBounds";
import type { Feature, LineString } from "geojson";

export interface RouteData {
  data: Feature<LineString>;
  points: GpxEntry[];
  gradient: unknown[];
  gradientEnabled: boolean;
  color: string;
  width: number;
  visible: boolean;
  index: number;
}

export interface MapDataGlobals {
  center: FlatPosition;
  isGradientEnabled: boolean;
}

export type MapDataActions = {
  setVisibility: (index: number, visible: boolean) => void;
  setColor: (index: number, color: string) => void;
  setGradient: (index: number, enabled: boolean) => void;
  setWidth: (index: number, width: number) => void;
  toogleGlobalGradient: (overrideState?: boolean) => void;
};

export type HoverTarget = { kind: "route" | "peak" | "photo"; id: number };

export const DefaultWidth = 4;
export const DefaultVisibility = true;
