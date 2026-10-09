import type { RouteData } from "@/components/MapVisualisations/UseMapData";
import { arrayUtils } from "../arrayUtils";

export type Bounds = [FlatPosition, FlatPosition]; // [[minLon, minLat], [maxLon, maxLat]]
///x/y or lon/lat
export type FlatPosition = [number, number];

export function getRoutesBounds(routes: RouteData[]): Bounds {
  const points = routes.flatMap((r) => r.points);
  return [
    [arrayUtils.min(points, (p) => p.lon), arrayUtils.min(points, (p) => p.lat)],
    [arrayUtils.max(points, (p) => p.lon), arrayUtils.max(points, (p) => p.lat)],
  ];
}

export function getRoutesCenter(baseRoutes: RouteData[]): FlatPosition {
  const merged = baseRoutes.flatMap((r) => r.points);
  const centerX = arrayUtils.average(merged, (p) => p.lon);
  const centerY = arrayUtils.average(merged, (p) => p.lat);

  return [centerX, centerY];
}

export function getRouteBoundsAndCenter(routes: RouteData[]) {
  //optimize it maybe someday but who cares really if its not throwing
  return {
    bounds: getRoutesBounds(routes),
    center: getRoutesCenter(routes),
  };
}
