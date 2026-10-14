import { IconClose } from "@/Icons/Icons";
import CameraControls from "@/components/MapVisualisations/CameraControls/CameraControls";
import useCameraControls from "@/components/MapVisualisations/CameraControls/useCameraControls";
import LayerControls from "@/components/MapVisualisations/LayerControls/LayerControls";
import useLayerControls from "@/components/MapVisualisations/LayerControls/useLayerControls";
import { MapDataProvider } from "@/components/MapVisualisations/MapData/MapDataProvider";
import MapVisualisation from "@/components/MapVisualisations/MapVisualisation";
import type { RoutePath } from "@/components/Trip/Details/RouteVisualisation/Dev/DEV_RouteVisualisationt";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { SecondaryButton } from "@/components/ui/Buttons";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { ResourceUrl } from "@/types/Api/types";
import { Box, Drawer } from "@chakra-ui/react";
import "maplibre-gl/dist/maplibre-gl.css";
import { RMapContextProvider } from "maplibre-react-components";
import MapRouteFilters from "./RouteFilters/MapRouteFilters";

export default function RegionRoutesVisualization({ data }: { data: ResourceUrl }) {
  const request = useResourceLink<RoutePath[]>(data);
  return (
    <FetchWrapper request={request}>{(routes) => <RegionRoutesMap paths={routes} />}</FetchWrapper>
  );
}

function RegionRoutesMap({ paths: routes }: { paths: RoutePath[] }) {
  const { form, options } = useCameraControls();
  const { form: layerForm, options: layerOptions } = useLayerControls();

  return (
    <MapDataProvider routes={routes}>
      <Drawer.Root
        modal={false}
        trapFocus={false}
        preventScroll={false}
        closeOnInteractOutside={false}
        placement="end"
      >
        <RMapContextProvider>
          <Box position="relative" overflow="hidden">
            <MapVisualisation cameraOptions={options} layerOptions={layerOptions} />
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
                <Drawer.Body padding={0}>
                  <Drawer.CloseTrigger asChild position="static">
                    <SecondaryButton size={"sm"} variant={"subtle"}>
                      <IconClose />
                    </SecondaryButton>
                  </Drawer.CloseTrigger>
                  <MapRouteFilters />
                  <CameraControls form={form} options={options} />
                  <LayerControls form={layerForm} />
                </Drawer.Body>
              </Drawer.Content>
            </Drawer.Positioner>
          </Box>
        </RMapContextProvider>
      </Drawer.Root>
    </MapDataProvider>
  );
}
