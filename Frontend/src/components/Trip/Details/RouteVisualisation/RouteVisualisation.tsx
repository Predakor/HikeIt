import { arrayUtils } from "@/Utils/arrayUtils";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { GpxEntry } from "@/types/Api/gpx.types";
import type { ResourceUrl } from "@/types/Api/types";
import { Stack } from "@chakra-ui/react";
import type { Feature, LineString } from "geojson";
import "maplibre-gl/dist/maplibre-gl.css"; // See notes below
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";

const rasterDemTiles = [
  "https://api.maptiler.com/tiles/terrain-rgb-v2/{z}/{x}/{y}.webp?key=ujaK4vTFjQkSEF2NqowK",
];
type RoutePath = { points: GpxEntry[] };

export default function RouteVisualisation({ data }: { data: ResourceUrl }) {
  const getElevationData = useResourceLink<RoutePath>(data);

  return (
    <Stack>
      <FetchWrapper request={getElevationData}>
        {({ points }) => {
          const center = {
            Lon: arrayUtils.average(points, (p) => p.lon),
            Lat: arrayUtils.average(points, (p) => p.lat),
          };

          const geoDatA: Feature<LineString> = {
            type: "Feature",
            geometry: {
              type: "LineString",
              coordinates: points.map((p) => [p.lon, p.lat]),
            },
            properties: null,
          };
          return (
            <RMap
              minZoom={12}
              initialPitch={60}
              initialBearing={-20}
              initialCenter={[center.Lon, center.Lat]}
              style={{ width: "100%", height: "700px" }}
              mapStyle="https://tiles.openfreemap.org/styles/bright"
            >
              <RSource key="hike-path" id="hike-path" type="geojson" data={geoDatA} />
              <RLayer
                type="line"
                source="hike-path"
                id="hike-line"
                paint={{
                  "line-color": "#FF0000", // Change this hex code to your preferred color
                  "line-width": 1, // Thickness in pixels
                  "line-opacity": 0.2, // 0 to 1 (transparency)
                }}
              />

              <RSource
                type="raster-dem"
                id="hillshade-data"
                tiles={rasterDemTiles}
                tileSize={512}
                maxzoom={14}
              />
              <RLayer id="hillshade" type="hillshade" source="hillshade-data" />

              <RSource
                type="raster-dem"
                id="terrain-mesh-data"
                tiles={rasterDemTiles}
                tileSize={512}
                maxzoom={12}
              />
              <RTerrain source="terrain-mesh-data" exaggeration={1.3} />
            </RMap>
          );
        }}
      </FetchWrapper>
    </Stack>
  );
}
