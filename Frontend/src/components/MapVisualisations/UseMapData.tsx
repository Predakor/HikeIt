import getRandomColor from "@/Utils/Colors/RouteColorGenerator";
import { buildSlopeGradientExpression } from "@/Utils/Colors/SlopeColorGenerator";
import toLineStringFeature from "@/Utils/Maps/toLineString";
import { arrayUtils } from "@/Utils/arrayUtils";
import type { GpxEntry } from "@/types/Api/gpx.types";
import type { Feature, LineString } from "geojson";
import { useMemo, useRef, useState } from "react";
import type { RoutePath } from "../Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";

export interface MapDataState {
  routes: RouteData[];
  center: [number, number];
  actions: MapDataActions;
}

export interface RouteData {
  data: Feature<LineString>;
  points: GpxEntry[];
  gradient: unknown[];
  gradientEnabled: boolean;
  color: string;
  visible: boolean;
  index: number;
}

type MapDataActions = {
  setVisibility: (index: number, visible: boolean) => void;
  setColor: (index: number, color: string) => void;
  setGradient: (index: number, enabled: boolean) => void;
  toogleGlobalGradient: (overrideState?: boolean) => void;
};

export default function useMapData(routes: RoutePath[]) {
  const baseRoutes: RouteData[] = useMemo(
    () =>
      routes.map(({ points }, index) => ({
        data: toLineStringFeature(points),
        points,
        gradientEnabled: false,
        gradient: buildSlopeGradientExpression(points),
        color: getRandomColor(index),
        visible: true,
        index,
      })),
    [routes],
  );

  // cheap, interactive — only holds what's been overridden
  const [globalGradientEnabled, setGlobalGradientEnabled] = useState(false);
  const [gradientsOverrides, setGradientOverrides] = useState<Record<number, boolean>>({});
  const [visibilityOverrides, setVisibilityOverrides] = useState<Record<number, boolean>>({});
  const [colorOverrides, setColorOverrides] = useState<Record<number, string>>({});

  // cheap merge — no distance math, just spreads
  const routesForRender: RouteData[] = useMemo(
    () =>
      baseRoutes.map((r) => ({
        ...r,
        visible: visibilityOverrides[r.index] ?? true,
        color: colorOverrides[r.index] ?? r.color,
        gradientEnabled: gradientsOverrides[r.index] ?? globalGradientEnabled,
      })),
    [baseRoutes, visibilityOverrides, colorOverrides, gradientsOverrides, globalGradientEnabled],
  );

  const center = useMemo(() => {
    if (!baseRoutes.length) return undefined;
    const merged = baseRoutes.flatMap((r) => r.points);
    return [arrayUtils.average(merged, (p) => p.lon), arrayUtils.average(merged, (p) => p.lat)] as [
      number,
      number,
    ];
  }, [baseRoutes]);

  const setVisibility = (index: number, visible: boolean) => {
    return setVisibilityOverrides((prev) => ({ ...prev, [index]: visible }));
  };

  const setColor = (index: number, color: string) => {
    return setColorOverrides((prev) => ({ ...prev, [index]: color }));
  };

  const setGradient = (index: number, enabled: boolean) => {
    return setGradientOverrides((prev) => ({ ...prev, [index]: enabled }));
  };

  const toogleGlobalGradient = (overrideState?: boolean) => {
    return setGlobalGradientEnabled((p) => (overrideState === undefined ? !p : overrideState));
  };

  const actions = useRef({
    setVisibility,
    setColor,
    setGradient,
    toogleGlobalGradient,
  } as MapDataActions);

  return {
    routes: routesForRender,
    center,
    actions: actions.current,
  } as MapDataState;
}
