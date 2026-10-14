import getRandomColor from "@/Utils/Colors/RouteColorGenerator";
import { buildSlopeGradientExpression } from "@/Utils/Colors/SlopeColorGenerator";
import { getRoutesCenter, type FlatPosition } from "@/Utils/Maps/routeBounds";
import toLineStringFeature from "@/Utils/Maps/toLineString";
import type { RoutePath } from "@/components/Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";
import { useMemo, useReducer, useState, type ReactNode } from "react";
import { HoverStoreContext, MapActionsContext, MapGlobalsContext, MapRoutesContext } from "./hooks";
import { createHoverStore } from "./hoverStore";
import {
  DefaultVisibility,
  DefaultWidth,
  type MapDataActions,
  type MapDataGlobals,
  type RouteData,
} from "./mapData.types";
import { initialMapDataOverrides, mapDataReducer } from "./mapDataReducer";

interface Props {
  routes: RoutePath[];
  children: ReactNode;
}

export function MapDataProvider({ routes, children }: Props) {
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

  const [overrides, dispatch] = useReducer(mapDataReducer, initialMapDataOverrides);
  const [hoverStore] = useState(createHoverStore);

  // cheap merge — no distance math, just spreads
  const routesForRender: RouteData[] = useMemo(
    () =>
      baseRoutes.map((r) => ({
        ...r,
        visible: overrides.visibility[r.index] ?? DefaultVisibility,
        color: overrides.color[r.index] ?? r.color,
        gradientEnabled: overrides.gradient[r.index] ?? overrides.globalGradient,
        width: overrides.width[r.index] ?? DefaultWidth,
      })),
    [baseRoutes, overrides],
  );

  const center = useMemo(() => {
    if (!baseRoutes.length) {
      return [0, 0];
    }

    return getRoutesCenter(baseRoutes);
  }, [baseRoutes]) satisfies FlatPosition;

  const globals = useMemo(
    () =>
      ({
        center,
        isGradientEnabled: overrides.globalGradient,
      }) satisfies MapDataGlobals,
    [center, overrides.globalGradient],
  );

  // built once: dispatch is stable, so consumers of actions never re-render from it
  const actions = useMemo(
    () =>
      ({
        setVisibility: (index, visible) => dispatch({ type: "visibility", index, value: visible }),
        setColor: (index, color) => dispatch({ type: "color", index, value: color }),
        setGradient: (index, enabled) => dispatch({ type: "gradient", index, value: enabled }),
        setWidth: (index, width) => dispatch({ type: "width", index, value: width }),
        toogleGlobalGradient: (overrideState) =>
          dispatch({ type: "globalGradient", value: overrideState }),
      }) satisfies MapDataActions,
    [],
  );

  return (
    <HoverStoreContext value={hoverStore}>
      <MapActionsContext value={actions}>
        <MapGlobalsContext value={globals}>
          <MapRoutesContext value={routesForRender}>{children}</MapRoutesContext>
        </MapGlobalsContext>
      </MapActionsContext>
    </HoverStoreContext>
  );
}
