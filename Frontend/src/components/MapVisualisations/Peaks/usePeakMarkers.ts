import { UseDetailedRegionPeaks } from "@/hooks/Regions/UseRegionPeaks";
import useUserRegion from "@/hooks/Regions/UseRegionProgress";
import type { PeakWithLocation } from "@/types/Api/peak.types";
import { downsampleToMaxSize } from "@/Utils/arrayUtils";
import { isNearAnyRoute } from "@/Utils/Maps/proximity";
import { useMemo } from "react";
import { useParams } from "react-router";
import { useMapRoutes } from "../MapData/hooks";
import { useLayerOptions } from "../MapSettings/hooks";

export const NEAR_ROUTE_KM = 2;
const MAX_ROUTE_POINTS = 500;

export type PeakMarkerData = PeakWithLocation & { reached: boolean };

export function usePeakMarkers(): PeakMarkerData[] {
  const { regionId } = useParams();
  const routes = useMapRoutes();
  const { allPeaks: showAllPeaks } = useLayerOptions();
  const peaksQuery = UseDetailedRegionPeaks(Number(regionId));
  const progressQuery = useUserRegion();

  const peaks = peaksQuery.data?.peaks;
  const progressPeaks = progressQuery.data?.progress.peaks;

  // a region without peaks makes the detailed query error, so missing data means no markers
  const sampledRoutes = useMemo(
    () =>
      routes.map((route) => ({
        ...route,
        points: downsampleToMaxSize(route.points, MAX_ROUTE_POINTS),
      })),
    [routes],
  );

  return useMemo(() => {
    if (!peaks) {
      return [];
    }

    const reachedById = new Map((progressPeaks ?? []).map((peak) => [peak.id, peak.reached]));

    return peaks
      .map((peak) => ({ ...peak, reached: reachedById.get(peak.id) ?? false }))
      .filter((peak) => {
        const isAlwaysShown = peak.reached || showAllPeaks;
        if (isAlwaysShown) {
          return true;
        }
        return isNearAnyRoute(peak, sampledRoutes, NEAR_ROUTE_KM);
      });
  }, [peaks, progressPeaks, sampledRoutes, showAllPeaks]);
}
