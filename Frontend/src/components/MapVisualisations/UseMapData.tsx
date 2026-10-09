import getRandomColor from "@/Utils/Colors/RouteColorGenerator";
import { buildSlopeGradientExpression } from "@/Utils/Colors/SlopeColorGenerator";
import { getRoutesCenter, type FlatPosition } from "@/Utils/Maps/routeBounds";
import toLineStringFeature from "@/Utils/Maps/toLineString";
import type { GpxEntry } from "@/types/Api/gpx.types";
import type { Feature, LineString } from "geojson";
import { useMemo, useRef, useState } from "react";
import type { RoutePath } from "../Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";

export interface MapDataState {
  routes: RouteData[];
  globals: MapDataGlobalSettings;
  actions: MapDataActions;
}

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

type MapDataActions = {
  setVisibility: (index: number, visible: boolean) => void;
  setColor: (index: number, color: string) => void;
  setGradient: (index: number, enabled: boolean) => void;
  setWidth: (index: number, width: number) => void;
  setHovered: (index: number, state: boolean) => void;
  toogleGlobalGradient: (overrideState?: boolean) => void;
};

type MapDataGlobalSettings = {
  center: FlatPosition;
  isGradientEnabled: boolean;
  hoveredIndex?: number;
};

export const DefaultWidth = 4;
const DefaultVisibility = true;

export default function useMapData(routes: RoutePath[]) {
  const baseRoutes: RouteData[] = useMemo(
    () =>
      routes.map(
        ({ points }, index) =>
          ({
            data: toLineStringFeature(points),
            points,
            gradientEnabled: false,
            gradient: buildSlopeGradientExpression(points),
            color: getRandomColor(index),
            width: DefaultWidth,
            visible: DefaultVisibility,
            index,
          }) satisfies RouteData,
      ),
    [routes],
  );

  // cheap, interactive — only holds what's been overridden
  const [globalGradientEnabled, setGlobalGradientEnabled] = useState(false);
  const [hoveredIndex, setHoveredIndex] = useState<number>();
  const [gradientsOverrides, setGradientOverrides] = useState<Record<number, boolean>>({});
  const [visibilityOverrides, setVisibilityOverrides] = useState<Record<number, boolean>>({});
  const [widthOverrides, setWidthOverrides] = useState<Record<number, number>>({});
  const [colorOverrides, setColorOverrides] = useState<Record<number, string>>({});

  // cheap merge — no distance math, just spreads
  const routesForRender: RouteData[] = useMemo(
    () =>
      baseRoutes.map((r) => ({
        ...r,
        visible: visibilityOverrides[r.index] ?? DefaultVisibility,
        color: colorOverrides[r.index] ?? r.color,
        gradientEnabled: gradientsOverrides[r.index] ?? globalGradientEnabled,
        width: widthOverrides[r.index] ?? DefaultWidth,
      })),
    [
      baseRoutes,
      visibilityOverrides,
      colorOverrides,
      widthOverrides,
      gradientsOverrides,
      globalGradientEnabled,
      hoveredIndex,
    ],
  );

  const center = useMemo(() => {
    if (!baseRoutes.length) {
      return [0, 0];
    }

    return getRoutesCenter(baseRoutes);
  }, [baseRoutes]) satisfies FlatPosition;

  const setVisibility = (index: number, visible: boolean) => {
    return setVisibilityOverrides((prev) => ({ ...prev, [index]: visible }));
  };

  const setColor = (index: number, color: string) => {
    return setColorOverrides((prev) => ({ ...prev, [index]: color }));
  };

  const setGradient = (index: number, enabled: boolean) => {
    return setGradientOverrides((prev) => ({ ...prev, [index]: enabled }));
  };

  const setWidth = (index: number, width: number) => {
    return setWidthOverrides((prev) => ({ ...prev, [index]: width }));
  };

  const setHovered = (index: number, state: boolean) => {
    return setHoveredIndex(state ? index : undefined);
  };

  const toogleGlobalGradient = (overrideState?: boolean) => {
    return setGlobalGradientEnabled((p) => (overrideState === undefined ? !p : overrideState));
  };

  const actions = useRef({
    setVisibility,
    setColor,
    setGradient,
    setWidth,
    setHovered,
    toogleGlobalGradient,
  } satisfies MapDataActions);

  const globals = useMemo(
    () =>
      ({
        center: center!,
        isGradientEnabled: globalGradientEnabled,
        hoveredIndex: hoveredIndex,
      }) satisfies MapDataGlobalSettings,
    [globalGradientEnabled, hoveredIndex],
  );

  return {
    routes: routesForRender,
    globals,
    actions: actions.current,
  } satisfies MapDataState;
}
