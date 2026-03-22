import { arrayUtils } from "@/Utils/arrayUtils";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { PrimaryButton, SecondaryButton } from "@/components/ui/Buttons";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { GpxEntry } from "@/types/Api/gpx.types";
import type { ResourceUrl } from "@/types/Api/types";
import { Popover, Portal, Show, Stack } from "@chakra-ui/react";
import type { Feature, LineString } from "geojson";
import "maplibre-gl/dist/maplibre-gl.css";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import { useState } from "react";
import { VisualisationFilterMenu } from "./VisualisationFilterMenu";
import { IconSettings } from "@/Icons/Icons";

const rasterDemTiles = [
  "https://api.maptiler.com/tiles/terrain-rgb-v2/{z}/{x}/{y}.webp?key=ujaK4vTFjQkSEF2NqowK",
];
export type RoutePath = { points: GpxEntry[] };

export default function DevRouteVisualisation({ data }: { data: ResourceUrl }) {
  const getElevationData = useResourceLink<RoutePath>(data);

  const [previewRoute, setPreviewRoute] = useState<Feature<LineString> | null>(null);

  return (
    <Stack>
      <Popover.Root positioning={{ placement: "bottom-end" }}>
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
                <Popover.Trigger position={"absolute"} margin={"2"} right={0}>
                  <SecondaryButton variant={"subtle"}>
                    <IconSettings />
                  </SecondaryButton>
                </Popover.Trigger>
                <RSource key="hike-path" id="hike-path" type="geojson" data={geoDatA} />
                <RLayer
                  type="line"
                  source="hike-path"
                  id="hike-line"
                  paint={{
                    "line-color": "blue",
                    "line-width": 2,
                    "line-opacity": 0.8,
                  }}
                />

                <Show when={previewRoute}>
                  <RSource
                    key="hike-path-preview"
                    id="hike-path-preview"
                    type="geojson"
                    data={previewRoute!}
                  />
                  <RLayer
                    type="line"
                    source="hike-path-preview"
                    id="hike-line-preview"
                    paint={{
                      "line-color": "#FF0000",
                      "line-width": 2,
                      "line-opacity": 0.8,
                    }}
                  />
                </Show>

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
        <Portal>
          <Popover.Positioner>
            <Popover.Content width={{ base: "90vw", md: "auto" }}>
              <Popover.Body>
                <VisualisationFilterMenu onFilterSubmit={setPreviewRoute} />
              </Popover.Body>
            </Popover.Content>
          </Popover.Positioner>
        </Portal>
      </Popover.Root>
    </Stack>
  );
}
