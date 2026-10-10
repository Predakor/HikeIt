import { IconClose } from "@/Icons/Icons";
import useCameraControls from "@/components/MapVisualisations/CameraControls/useCameraControls";
import MapVisualisation from "@/components/MapVisualisations/MapVisualisation";
import useMapData from "@/components/MapVisualisations/useMapData";
import type { RoutePath } from "@/components/Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { SecondaryButton } from "@/components/ui/Buttons";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { ResourceUrl } from "@/types/Api/types";
import { Box, Drawer } from "@chakra-ui/react";
import "maplibre-gl/dist/maplibre-gl.css";
import { RMapContextProvider } from "maplibre-react-components";
import RegionRoutesFilters from "./RegionRoutesFilters";

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
    <Drawer.Root
      modal={false}
      trapFocus={false}
      preventScroll={false}
      closeOnInteractOutside={false}
      placement="end"
    >
      <RMapContextProvider>
        <Box position="relative" overflow="hidden">
          <MapVisualisation data={mapData} cameraOptions={cameraOptions} />
          <Drawer.Positioner
            pos={"absolute"}
            inset="0"
            width="full"
            height="full"
            pointerEvents="none"
          >
            <Drawer.Content
              padding={2}
              bg="blackAlpha.700"
              backdropFilter="blur(4px) saturate(160%)"
              boxShadow="lg"
            >
              <Drawer.Body asChild>
                <RegionRoutesFilters
                  mapData={mapData}
                  controlButton={
                    <Drawer.CloseTrigger asChild position="static">
                      <SecondaryButton size={"sm"} variant={"subtle"}>
                        <IconClose />
                      </SecondaryButton>
                    </Drawer.CloseTrigger>
                  }
                />
              </Drawer.Body>
            </Drawer.Content>
          </Drawer.Positioner>
        </Box>
      </RMapContextProvider>
    </Drawer.Root>
  );
}
