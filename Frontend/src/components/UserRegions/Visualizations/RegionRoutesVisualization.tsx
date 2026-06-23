import { IconSettings } from "@/Icons/Icons";
import getRandomColor from "@/Utils/Colors/RouteColorGenerator";
import { arrayUtils } from "@/Utils/arrayUtils";
import type { RoutePath } from "@/components/Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";
import { rasterDemTiles } from "@/components/Trip/Details/RouteVisualisation/RouteVisualisation";
import { SecondaryButton } from "@/components/ui/Buttons";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { ResourceUrl } from "@/types/Api/types";
import { For, Popover, Portal, Skeleton } from "@chakra-ui/react";
import type { Feature, LineString } from "geojson";
import "maplibre-gl/dist/maplibre-gl.css";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import { useEffect, useState } from "react";
import RegionRoutesFilters from "./RegionRoutesFilters";

export interface RouteData {
  data: Feature<LineString>;
  color: string;
  visible: boolean;
  index: number;
}

export default function RegionRoutesVisualization({ data }: { data: ResourceUrl }) {
  const request = useResourceLink<RoutePath[]>(data);
  const [routes, setRoutes] = useState<RouteData[]>([]);
  const [center, setCenter] = useState<[number, number]>();
  const [exaggeration, setExaggeration] = useState(1.3);

  useEffect(() => {
    if (!request.data || request.data.length == 0) {
      return;
    }

    const mergedPoints = request.data.flatMap((x) => x.points);
    setCenter([
      arrayUtils.average(mergedPoints, (p) => p.lon),
      arrayUtils.average(mergedPoints, (p) => p.lat),
    ]);

    const processedRoutes: RouteData[] = request.data.map(({ points }, index) => ({
      data: {
        type: "Feature",
        geometry: {
          type: "LineString",
          coordinates: points.map((p) => [p.lon, p.lat]),
        },
        properties: null,
      },
      color: getRandomColor(index),
      visible: true,
      index,
    }));

    setRoutes(processedRoutes);
  }, [request.data]);

  if (!routes?.length) {
    return <Skeleton width={"100%"} height={"700px"}></Skeleton>;
  }

  return (
    <Popover.Root>
      <RMap
        minZoom={12}
        initialPitch={60}
        initialBearing={-20}
        initialCenter={center}
        style={{ width: "100%", height: "700px" }}
        mapStyle="https://tiles.openfreemap.org/styles/bright"
      >
        <Popover.Trigger position={"absolute"} margin={"2"} right={0} asChild>
          <SecondaryButton variant={"subtle"}>
            <IconSettings />
          </SecondaryButton>
        </Popover.Trigger>

        <For each={routes}>
          {(route) => (
            <>
              <RSource
                key={route.index}
                id={`hike-path-${route.index}`}
                type="geojson"
                data={route.data}
              />
              <RLayer
                type="line"
                source={`hike-path-${route.index}`}
                id={`hike-line-${route.index}`}
                paint={{
                  "line-color": route.color,
                  "line-width": 4,
                  "line-opacity": route.visible ? 1 : 0,
                }}
              />
            </>
          )}
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
        <RTerrain source="terrain-mesh-data" exaggeration={exaggeration} />
      </RMap>
      <Portal>
        <Popover.Positioner>
          <Popover.Content width={{ base: "90vw", md: "auto" }}>
            <Popover.Body>
              <RegionRoutesFilters
                routes={routes}
                setRoutes={setRoutes}
                exageration={exaggeration}
                setExageration={setExaggeration}
              />
            </Popover.Body>
          </Popover.Content>
        </Popover.Positioner>
      </Portal>
    </Popover.Root>
  );
}
