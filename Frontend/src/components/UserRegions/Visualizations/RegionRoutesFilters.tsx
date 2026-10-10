import { type MapDataState } from "@/components/MapVisualisations/useMapData";
import { PrimaryButton } from "@/components/ui/Buttons";
import { Flex, For, GridItem, Input, SimpleGrid, Stack } from "@chakra-ui/react";
import { type ReactElement } from "react";
import RouteFilter from "./RouteFilter/RouteFilter";

interface Props {
  mapData: MapDataState;
  controlButton?: ReactElement;
}

export default function RegionRoutesFilters({ mapData: map, controlButton }: Props) {
  const { routes, globals, actions } = map;

  return (
    <Stack
      direction={{
        lg: "row",
      }}
    >
      <SimpleGrid columns={2} gap={"4"} alignContent={"start"}>
        <GridItem colSpan={2}>
          <Flex justify={"space-between"}>
            <PrimaryButton onClick={() => actions.toogleGlobalGradient()} size={"sm"}>
              {globals.isGradientEnabled ? "Slope coloring: On" : "Slope coloring: Off"}
            </PrimaryButton>
            {controlButton}
          </Flex>
        </GridItem>

        <GridItem colSpan={2} asChild>
          <Stack>
            <For each={routes}>{(route) => <RouteFilter route={route} actions={actions} />}</For>
          </Stack>
        </GridItem>

        <Input type="range" value={1} onChange={() => {}} min={0.1} max={2} step={0.01} />
      </SimpleGrid>
    </Stack>
  );
}
