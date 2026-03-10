import { arrayUtils } from "@/Utils/arrayUtils";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { GpxEntry } from "@/types/Api/gpx.types";
import type { ResourceUrl } from "@/types/Api/types";
import { RMap, RSource, RLayer, RTerrain } from "maplibre-react-components";
import type { Feature, LineString } from "geojson";
import "maplibre-gl/dist/maplibre-gl.css"; // See notes below

export default function RouteVisualisation({ data }: { data: ResourceUrl }) {
  const getElevationData = useResourceLink<{ points: GpxEntry[] }>(data);
  const rasterDemTiles = [
    "https://api.maptiler.com/tiles/terrain-rgb-v2/{z}/{x}/{y}.webp?key=ujaK4vTFjQkSEF2NqowK",
  ];
  return (
    <FetchWrapper request={getElevationData}>
      {({ points }) => {
        const center = {
          Lon: arrayUtils.average(points, (p) => p.lon),
          Lat: arrayUtils.average(points, (p) => p.lat),
        };

        const coordinates = points.map((p) => [p.lon, p.lat]);
        const geoDate: Feature<LineString> = {
          type: "Feature",
          geometry: {
            type: "LineString",
            coordinates: coordinates,
          },
          properties: null,
        };
        return (
          <RMap
            minZoom={12}
            initialPitch={60}
            initialBearing={-20}
            initialCenter={[center.Lon, center.Lat]}
            style={{ width: 1200, height: 800 }}
            mapStyle="https://tiles.openfreemap.org/styles/bright"
          >
            <RSource key="hike-path" id="hike-path" type="geojson" data={geoDate} />
            <RLayer type="line" source="hike-path" id="hike-line" />

            <RSource type="raster-dem" id="terrarium" tiles={rasterDemTiles} tileSize={256} />
            <RLayer id="hillshade" type="hillshade" source="terrarium" />
            <RTerrain source="terrarium" exaggeration={1.5} />
          </RMap>
        );
      }}
    </FetchWrapper>
  );
}
