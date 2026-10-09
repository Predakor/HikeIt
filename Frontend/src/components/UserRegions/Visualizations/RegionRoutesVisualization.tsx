import useCameraControls from "@/components/MapVisualisations/CameraControls/useCameraControls";
import MapVisualisation from "@/components/MapVisualisations/MapVisualisation";
import useMapData from "@/components/MapVisualisations/UseMapData";
import type { RoutePath } from "@/components/Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { GpxEntry } from "@/types/Api/gpx.types";
import type { ResourceUrl } from "@/types/Api/types";
import { Popover } from "@chakra-ui/react";
import { Portal } from "@chakra-ui/react/portal";
import type { Feature, LineString } from "geojson";
import "maplibre-gl/dist/maplibre-gl.css";
import RegionRoutesFilters from "./RegionRoutesFilters";
import { RMapContextProvider } from "maplibre-react-components";

export interface RouteData {
  data: Feature<LineString>;
  points: GpxEntry[];
  gradient: unknown[];
  color: string;
  visible: boolean;
  index: number;
}

export default function RegionRoutesVisualization({ data }: { data: ResourceUrl }) {
  const request = useResourceLink<RoutePath[]>(data);
  // hooks would live here for example and passed down
  return (
    <FetchWrapper request={request}>{(routes) => <RegionRoutesMap paths={routes} />}</FetchWrapper>
  );
}

function RegionRoutesMap({ paths: routes }: { paths: RoutePath[] }) {
  const mapData = useMapData(routes);
  const [cameraOptions, cameraControls] = useCameraControls();

  return (
    <>
      <Popover.Root>
        <RMapContextProvider>
          <MapVisualisation data={mapData} cameraOptions={cameraOptions} />
          <Portal>
            <Popover.Positioner>
              <Popover.Content width={{ base: "90vw", md: "auto" }}>
                <Popover.Body>
                  <RegionRoutesFilters mapData={mapData} />
                </Popover.Body>
              </Popover.Content>
            </Popover.Positioner>
          </Portal>
        </RMapContextProvider>
      </Popover.Root>
    </>
  );
}
