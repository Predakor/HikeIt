import "maplibre-gl/dist/maplibre-gl.css";
import { RLayer, RMap, RSource, RTerrain } from "maplibre-react-components";
import type { ReactNode } from "react";
import { rasterDemTiles } from "./rasterDemTiles";

interface TerrainMapProps {
  center: [lon: number, lat: number];
  height: string;
  // Route layers and controls; drawn below the hillshade, like the terrain sources they sit on.
  children?: ReactNode;
}

export function TerrainMap({ center, height, children }: TerrainMapProps) {
  return (
    <RMap
      minZoom={12}
      initialPitch={60}
      initialBearing={-20}
      initialCenter={center}
      style={{ width: "100%", height }}
      mapStyle="https://tiles.openfreemap.org/styles/bright"
    >
      {children}

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
}
