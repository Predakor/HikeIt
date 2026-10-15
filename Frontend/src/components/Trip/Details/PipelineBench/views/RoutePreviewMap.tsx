import { arrayUtils } from "@/Utils/arrayUtils";
import type { GpxEntry } from "@/types/Api/gpx.types";
import { Text } from "@chakra-ui/react";
import type { FeatureCollection, LineString } from "geojson";
import "maplibre-gl/dist/maplibre-gl.css";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import { memo, useMemo } from "react";
import type { RoutePreviewMapProps } from "../bench.types";

const rasterDemTiles = [
  "https://api.maptiler.com/tiles/terrain-rgb-v2/{z}/{x}/{y}.webp?key=ujaK4vTFjQkSEF2NqowK",
];

function RoutePreviewMap({ baseline, preview }: RoutePreviewMapProps) {
  const baselineRoute = useMemo(() => toRouteCollection(baseline?.points), [baseline?.points]);
  const previewRoute = useMemo(() => toRouteCollection(preview?.points), [preview?.points]);

  const baselinePoints = baseline?.points ?? [];
  const previewPoints = preview?.points ?? [];
  const hasBaseline = baselinePoints.length > 0;
  const hasPreview = previewPoints.length > 0;

  if (!hasBaseline && !hasPreview) {
    return <Text color={"fg.muted"}>No data</Text>;
  }

  // Centre on the preview route when there is one, otherwise on the baseline.
  const centreRoute = hasPreview ? previewPoints : baselinePoints;
  const center = {
    Lon: arrayUtils.average(centreRoute, (p) => p.lon),
    Lat: arrayUtils.average(centreRoute, (p) => p.lat),
  };

  return (
    <RMap
      minZoom={12}
      initialPitch={60}
      initialBearing={-20}
      initialCenter={[center.Lon, center.Lat]}
      style={{ width: "100%", height: "600px" }}
      mapStyle="https://tiles.openfreemap.org/styles/bright"
    >
      <RSource id="bench-baseline-route" type="geojson" data={baselineRoute} />
      <RLayer
        type="line"
        source="bench-baseline-route"
        id="bench-baseline-line"
        paint={{
          "line-color": "gray",
          "line-width": 2,
          "line-opacity": 0.6,
        }}
      />

      <RSource id="bench-preview-route" type="geojson" data={previewRoute} />
      <RLayer
        type="line"
        source="bench-preview-route"
        id="bench-preview-line"
        paint={{
          "line-color": "blue",
          "line-width": 2,
          "line-opacity": 0.9,
        }}
      />

      <RSource
        type="raster-dem"
        id="bench-hillshade-data"
        tiles={rasterDemTiles}
        tileSize={512}
        maxzoom={14}
      />
      <RLayer id="bench-hillshade" type="hillshade" source="bench-hillshade-data" />

      <RSource
        type="raster-dem"
        id="bench-terrain-mesh-data"
        tiles={rasterDemTiles}
        tileSize={512}
        maxzoom={12}
      />
      <RTerrain source="bench-terrain-mesh-data" exaggeration={1.3} />
    </RMap>
  );
}

// Memoised so editing a config (which re-renders the bench) does not re-render the map until its data changes.
export default memo(RoutePreviewMap);

const emptyRoute: FeatureCollection<LineString> = { type: "FeatureCollection", features: [] };

// Empty collection when there is no route, so the source keeps a stable id and only its data changes.
function toRouteCollection(points: GpxEntry[] | undefined): FeatureCollection<LineString> {
  if (!points || points.length === 0) {
    return emptyRoute;
  }

  return {
    type: "FeatureCollection",
    features: [
      {
        type: "Feature",
        properties: {},
        geometry: {
          type: "LineString",
          coordinates: points.map((point) => [point.lon, point.lat]),
        },
      },
    ],
  };
}
