import { IconHidden, IconLocate, IconVisible } from "@/Icons/Icons";
import { getRouteBoundsAndCenter } from "@/Utils/Maps/routeBounds";
import { RegionRoutesMapId } from "@/components/MapVisualisations/MapVisualisation";
import type { MapDataState, RouteData } from "@/components/MapVisualisations/UseMapData";
import { PrimaryButton } from "@/components/ui/Buttons";
import { For, GridItem, IconButton, Input, SimpleGrid, Stack } from "@chakra-ui/react";
import { useMap } from "maplibre-react-components";

interface Props {
  mapData: MapDataState;
}

export default function RegionRoutesFilters({ mapData: map }: Props) {
  const { routes, globals, actions } = map;
  const mapHook = useMap(RegionRoutesMapId);

  const locateHandler = (route: RouteData) => {
    const { bounds } = getRouteBoundsAndCenter([route]);
    mapHook?.fitBounds(bounds, {
      linear: true,
      duration: 700,
    });
  };

  return (
    <Stack
      direction={{
        lg: "row",
      }}
    >
      <SimpleGrid columns={2} gap={"4"} maxWidth={"sm"} alignContent={"start"}>
        <GridItem colSpan={2}>
          <PrimaryButton onClick={() => actions.toogleGlobalGradient()}>
            {globals.isGradientEnabled ? "Slope coloring: On" : "Slope coloring: Off"}
          </PrimaryButton>
        </GridItem>

        <GridItem colSpan={2} asChild>
          <Stack>
            <For each={routes}>
              {(route) => (
                <Stack
                  direction={"row"}
                  onMouseEnter={() => actions.setHovered(route.index, true)}
                  onMouseLeave={() => actions.setHovered(route.index, false)}
                  key={route.index}
                >
                  <Input
                    height={"full"}
                    width={"auto"}
                    aspectRatio={"square"}
                    padding={0}
                    margin={0}
                    border={0}
                    type="color"
                    // disabled={slopeColoringEnabled}
                    defaultValue={route.color}
                    onChange={(e) => actions.setColor(route.index, e.target.value)}
                  />
                  <IconButton
                    aria-label="Show/Hide route"
                    onClick={() => actions.setVisibility(route.index, !route.visible)}
                  >
                    {route.visible ?
                      <IconVisible />
                    : <IconHidden />}
                  </IconButton>

                  <IconButton
                    aria-label="Locate route "
                    onClick={() => {
                      locateHandler(route);
                    }}
                  >
                    <IconLocate />
                  </IconButton>
                </Stack>
              )}
            </For>
          </Stack>
        </GridItem>

        <Input type="range" value={1} onChange={() => {}} min={0.1} max={2} step={0.01} />
      </SimpleGrid>
    </Stack>
  );
}
