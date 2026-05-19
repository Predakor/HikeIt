import { arrayUtils } from "@/Utils/arrayUtils";
import type { RoutePath } from "@/components/Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";
import { rasterDemTiles } from "@/components/Trip/Details/RouteVisualisation/RouteVisualisation";
import FetchWrapper from "@/components/Utils/Fetching";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { ResourceUrl } from "@/types/Api/types";
import { For } from "@chakra-ui/react";
import type { Feature, LineString } from "geojson";
import "maplibre-gl/dist/maplibre-gl.css";
import { RMap, RSource, RLayer, RTerrain } from "maplibre-react-components";

export default function RegionRoutesVisualization({ data }: { data: ResourceUrl }) {
  const request = useResourceLink<RoutePath[]>(data);

  return (
    <FetchWrapper request={request}>
      {(visualizations) => {
        const mergedPoints = visualizations.flatMap((x) => x.points);
        const center = {
          Lon: arrayUtils.average(mergedPoints, (p) => p.lon),
          Lat: arrayUtils.average(mergedPoints, (p) => p.lat),
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
            <For each={visualizations}>
              {({ points }, index) => {
                const geoDatA: Feature<LineString> = {
                  type: "Feature",
                  geometry: {
                    type: "LineString",
                    coordinates: points.map((p) => [p.lon, p.lat]),
                  },
                  properties: null,
                };
                return (
                  <>
                    <RSource key={index} id={"hike-path" + index} type="geojson" data={geoDatA} />
                    <RLayer
                      type="line"
                      source={"hike-path" + index}
                      id={"hike-line" + index}
                      paint={{
                        "line-color": "blue", // Change this hex code to your preferred color
                        "line-width": 4, // Thickness in pixels
                        "line-opacity": 1, // 0 to 1 (transparency)
                      }}
                    />
                  </>
                );
              }}
            </For>

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
  );
}
