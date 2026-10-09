import { IconSettings } from "@/Icons/Icons";
import { For, Popover } from "@chakra-ui/react";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import { rasterDemTiles } from "../Trip/Details/RouteVisualisation/RouteVisualisation";
import { SecondaryButton } from "../ui/Buttons";
import { type CameraOptions } from "./CameraControls/useCameraControls";
import { DefaultWidth, type MapDataState } from "./UseMapData";

interface Props {
  data: MapDataState;
  cameraOptions: CameraOptions;
}
export const RegionRoutesMapId = "region-routes-map";

export default function MapVisualisation({ data, cameraOptions }: Props) {
  return (
    <RMap
      id={RegionRoutesMapId}
      minZoom={cameraOptions.minZoom}
      initialPitch={cameraOptions.maxPitch - cameraOptions.minZoom}
      initialBearing={-20}
      initialCenter={data.globals.center}
      style={{ width: "100%", height: "700px" }}
      mapStyle="https://tiles.openfreemap.org/styles/bright"
    >
      <Popover.Trigger position={"absolute"} margin={"2"} right={0} asChild>
        <SecondaryButton variant={"subtle"}>
          <IconSettings />
        </SecondaryButton>
      </Popover.Trigger>

      <For each={data.routes}>
        {(route) => {
          const { hoveredIndex } = data.globals;
          const { index } = route;
          let opacity = 1;
          let width = DefaultWidth;
          if (hoveredIndex) {
            const widthMultiplier = route.index == hoveredIndex ? 2 : 0.5;
            width *= widthMultiplier;
          }

          if (!route.visible) {
            opacity = 0;
          }

          return (
            <>
              <RSource
                key={index}
                id={`hike-path-${index}`}
                type="geojson"
                data={route.data}
                lineMetrics
              />
              <RLayer
                type="line"
                source={`hike-path-${index}`}
                id={`hike-line-${index}`}
                paint={{
                  "line-gradient": route.gradientEnabled ? (route.gradient as any) : route.color,
                  "line-width": width,
                  "line-opacity": opacity,
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
        maxzoom={14}
      />
      <RTerrain source="terrain-mesh-data" exaggeration={1} />
    </RMap>
  );
}
