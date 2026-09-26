import type { GpxEntry } from "@/types/Api/gpx.types";
import type { Feature, LineString } from "geojson";

export default function toLineStringFeature(points: GpxEntry[]) {
  return {
    type: "Feature",
    geometry: {
      type: "LineString",
      coordinates: points.map((p) => [p.lon, p.lat]),
    },
    properties: null,
  } satisfies Feature<LineString>;
}
