import { useMutation } from "@tanstack/react-query";
import type { Feature, LineString } from "geojson";
import api from "@/Utils/Api/apiRequest";
import type { RoutePath } from "./DEV_RouteVisualisationt";
import type { GpxFilter } from "./VisualisationFilterMenu";

interface UsePreviewOptions {
  tripId?: string;
  onSuccess: (feature: Feature<LineString>) => void;
}

export function useVisualisationPreview({ tripId, onSuccess }: UsePreviewOptions) {
  return useMutation({
    mutationFn: (filters: GpxFilter[]) =>
      api.post<RoutePath>(`trips/${tripId}/analytics/visualizations/preview`, filters),
    onSuccess: ({ points }) => {
      onFilterSubmitInternal(points, onSuccess);
    },
  });
}

const onFilterSubmitInternal = (
  points: RoutePath["points"],
  callback: UsePreviewOptions["onSuccess"],
) => {
  callback({
    type: "Feature",
    geometry: {
      type: "LineString",
      coordinates: points.map((p) => [p.lon, p.lat]),
    },
    properties: null,
  });
};
