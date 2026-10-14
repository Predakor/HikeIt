import { IconSettings } from "@/Icons/Icons";
import { Drawer, For } from "@chakra-ui/react";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import { rasterDemTiles } from "../Trip/Details/RouteVisualisation/RouteVisualisation";
import { SecondaryButton } from "../ui/Buttons";
import { type CameraOptions } from "./CameraControls/useCameraControls";
import type { LayerOptions } from "./LayerControls/useLayerControls";
import type { MapDataState } from "./useMapData";

interface Props {
  data: MapDataState;
  cameraOptions: CameraOptions;
  layerOptions: LayerOptions;
}
export const RegionRoutesMapId = "region-routes-map";

export default function MapVisualisation({ data, cameraOptions, layerOptions }: Props) {
  return (
    <RMap
      id={RegionRoutesMapId}
      initialPitch={cameraOptions.maxPitch - cameraOptions.minZoom}
      initialZoom={cameraOptions.minZoom}
      initialBearing={-15}
      initialCenter={data.globals.center}
      style={{ width: "100%", height: "700px" }}
      mapStyle="https://tiles.openfreemap.org/styles/bright"
    >
      <Drawer.Trigger position={"absolute"} margin={"2"} right={0} asChild>
        <SecondaryButton variant={"subtle"}>
          <IconSettings />
        </SecondaryButton>
      </Drawer.Trigger>

      <For each={data.routes}>
        {(route) => {
          const { hoveredIndex } = data.globals;
          const { index } = route;
          let opacity = 1;
          let width = route.width;
          if (hoveredIndex !== undefined) {
            const widthMultiplier = route.index == hoveredIndex ? 2 : 0.5;
            width *= widthMultiplier;
          }

          if (!route.visible) {
            opacity = 0;
          }

          const paint = {
            "line-gradient": route.gradientEnabled ? (route.gradient as any) : route.color,
            "line-width": width,
            "line-opacity": opacity,
          };

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
                paint={paint}
              />
              {/* layers stack in the order they were added, so a copy mounted on hover draws above every route */}
              {hoveredIndex === index && route.visible && (
                <RLayer
                  type="line"
                  source={`hike-path-${index}`}
                  id={`hike-line-top-${index}`}
                  paint={paint}
                />
              )}
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
      <RLayer
        id="hillshade"
        type="hillshade"
        source="hillshade-data"
        layout={{ visibility: layerOptions.hillshade ? "visible" : "none" }}
      />
      <RSource
        type="raster-dem"
        id="terrain-mesh-data"
        tiles={rasterDemTiles}
        tileSize={512}
        maxzoom={14}
      />
      {layerOptions.terrain && (
        <RTerrain source="terrain-mesh-data" exaggeration={layerOptions.exaggeration} />
      )}
    </RMap>
  );
}
