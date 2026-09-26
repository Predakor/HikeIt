import type { MapDataState } from "@/components/MapVisualisations/UseMapData";
import { PrimaryButton } from "@/components/ui/Buttons";
import { For, GridItem, Input, SimpleGrid, Stack } from "@chakra-ui/react";

interface Props {
  mapData: MapDataState;
}

export default function RegionRoutesFilters({ mapData: map }: Props) {
  const { routes, actions } = map;
  return (
    <Stack
      direction={{
        lg: "row",
      }}
    >
      <SimpleGrid columns={2} gap={"4"} maxWidth={"sm"} alignContent={"start"}>
        <GridItem colSpan={2}>
          <PrimaryButton onClick={() => actions.toogleGlobalGradient()}>
            {true ? "Slope coloring: On" : "Slope coloring: Off"}
          </PrimaryButton>
        </GridItem>

        <GridItem colSpan={2} asChild>
          <Stack>
            <For each={routes}>
              {(route) => (
                <Stack direction={"row"} key={route.index}>
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
                  <PrimaryButton onClick={() => actions.setVisibility(route.index, !route.visible)}>
                    {route.visible ? "Hide" : "Show"}
                  </PrimaryButton>
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
