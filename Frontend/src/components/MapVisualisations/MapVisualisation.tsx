import { IconSettings } from "@/Icons/Icons";
import { Drawer, For } from "@chakra-ui/react";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import { memo } from "react";
import { rasterDemTiles } from "../Trip/Details/RouteVisualisation/RouteVisualisation";
import { SecondaryButton } from "../ui/Buttons";
import { DEFAULT_CAMERA_OPTIONS } from "./CameraControls/useCameraControls";
import { useHasHover, useIsHovered, useMapGlobals, useMapRoutes } from "./MapData/hooks";
import type { RouteData } from "./MapData/mapData.types";
import { useLayerOptions } from "./MapSettings/hooks";
import PeakMarkers from "./Peaks/PeakMarkers";

export const RegionRoutesMapId = "region-routes-map";

export default function MapVisualisation() {
  const routes = useMapRoutes();
  const globals = useMapGlobals();
  const layerOptions = useLayerOptions();

  return (
    <RMap
      id={RegionRoutesMapId}
      initialPitch={DEFAULT_CAMERA_OPTIONS.maxPitch - DEFAULT_CAMERA_OPTIONS.minZoom}
      initialZoom={DEFAULT_CAMERA_OPTIONS.minZoom}
      initialBearing={-15}
      initialCenter={globals.center}
      style={{ width: "100%", height: "700px" }}
      mapStyle="https://tiles.openfreemap.org/styles/bright"
    >
      <Drawer.Trigger position={"absolute"} margin={"2"} right={0} asChild>
        <SecondaryButton variant={"subtle"}>
          <IconSettings />
        </SecondaryButton>
      </Drawer.Trigger>

      <For each={routes}>{(route) => <RouteLayers key={route.index} route={route} />}</For>

      <PeakMarkers />

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

// subscribes to hover itself, so hover changes re-render only the layers involved
const RouteLayers = memo(function RouteLayers({ route }: { route: RouteData }) {
  const { index } = route;
  const isHovered = useIsHovered("route", index);
  const hasHover = useHasHover("route");

  let opacity = 1;
  let width = route.width;
  if (hasHover) {
    const widthMultiplier = isHovered ? 2 : 0.5;
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
      <RSource id={`hike-path-${index}`} type="geojson" data={route.data} lineMetrics />
      <RLayer
        type="line"
        source={`hike-path-${index}`}
        id={`hike-line-${index}`}
        paint={paint}
      />
      {/* layers stack in the order they were added, so a copy mounted on hover draws above every route */}
      {isHovered && route.visible && (
        <RLayer
          type="line"
          source={`hike-path-${index}`}
          id={`hike-line-top-${index}`}
          paint={paint}
        />
      )}
    </>
  );
});
