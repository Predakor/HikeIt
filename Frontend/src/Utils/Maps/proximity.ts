import type { RouteData } from "@/components/MapVisualisations/MapData/mapData.types";

const KM_PER_DEGREE_LAT = 110.57;
const KM_PER_DEGREE_LON_AT_EQUATOR = 111.32;

// equirectangular approximation, cheap enough for peaks x route points at region scale
export function isNearAnyRoute(
  position: { lat: number; lon: number },
  routes: RouteData[],
  maxKm: number,
): boolean {
  const maxKmSquared = maxKm * maxKm;
  const cosLat = Math.cos((position.lat * Math.PI) / 180);

  return routes.some((route) =>
    route.points.some((point) => {
      const dx = (point.lon - position.lon) * cosLat * KM_PER_DEGREE_LON_AT_EQUATOR;
      const dy = (point.lat - position.lat) * KM_PER_DEGREE_LAT;
      return dx * dx + dy * dy <= maxKmSquared;
    }),
  );
}
