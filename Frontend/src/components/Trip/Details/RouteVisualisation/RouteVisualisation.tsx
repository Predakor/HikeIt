import toLineStringFeature from "@/Utils/Maps/toLineString";
import { arrayUtils } from "@/Utils/arrayUtils";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { TerrainMap } from "@/components/MapVisualisations/TerrainMap";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { GpxEntry } from "@/types/Api/gpx.types";
import type { ResourceUrl } from "@/types/Api/types";
import { Stack } from "@chakra-ui/react";
import { RLayer, RSource } from "maplibre-react-components";

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

          return (
            <TerrainMap center={[center.Lon, center.Lat]} height={"700px"}>
              <RSource
                key="hike-path"
                id="hike-path"
                type="geojson"
                data={toLineStringFeature(points)}
              />
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
            </TerrainMap>
          );
        }}
      </FetchWrapper>
    </Stack>
  );
}
