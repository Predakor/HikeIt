import { IconSettings } from "@/Icons/Icons";
import { For, Popover } from "@chakra-ui/react";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import { rasterDemTiles } from "../Trip/Details/RouteVisualisation/RouteVisualisation";
import { SecondaryButton } from "../ui/Buttons";
import { type CameraOptions } from "./CameraControls/useCameraControls";
import { type MapDataState } from "./UseMapData";

interface Props {
  data: MapDataState;
  cameraOptions: CameraOptions;
}
export default function MapVisualisation({ data, cameraOptions }: Props) {
  return (
    <RMap
      minZoom={cameraOptions.minZoom}
      initialPitch={cameraOptions.maxPitch - cameraOptions.minZoom}
      initialBearing={-20}
      initialCenter={data.center}
      style={{ width: "100%", height: "700px" }}
      mapStyle="https://tiles.openfreemap.org/styles/bright"
    >
      <Popover.Trigger position={"absolute"} margin={"2"} right={0} asChild>
        <SecondaryButton variant={"subtle"}>
          <IconSettings />
        </SecondaryButton>
      </Popover.Trigger>

      <For each={data.routes}>
        {(route) => (
          <>
            <RSource
              key={route.index}
              id={`hike-path-${route.index}`}
              type="geojson"
              data={route.data}
              lineMetrics
            />
            <RLayer
              type="line"
              source={`hike-path-${route.index}`}
              id={`hike-line-${route.index}`}
              paint={
                route.gradientEnabled ?
                  {
                    "line-gradient": route.gradient,
                    "line-width": 4,
                    "line-opacity": route.visible ? 1 : 0, // replace with visible when added
                  }
                : {
                    "line-color": route.color,
                    "line-width": 4,
                    "line-opacity": route.visible ? 1 : 0, //replace with visible when added
                  }
              }
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
      <RTerrain source="terrain-mesh-data" exaggeration={1} />
    </RMap>
  );
}
