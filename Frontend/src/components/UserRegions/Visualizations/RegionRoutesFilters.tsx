import { debounce } from "@/Utils/debounce";
import { PrimaryButton } from "@/components/ui/Buttons";
import { For, GridItem, Input, SimpleGrid, Stack } from "@chakra-ui/react";
import { type ChangeEvent } from "react";
import type { RouteData } from "./RegionRoutesVisualization";

interface Props {
  routes: RouteData[];
  setRoutes: (data: RouteData[]) => void;
  exageration: number;
  setExageration: (number: number) => void;
}

export default function RegionRoutesFilters({
  routes,
  setRoutes,
  exageration,
  setExageration,
}: Props) {
  const handleVisibilityChange = (route: RouteData) => {
    const newData = routes.map((d) => {
      return d.index === route.index ? { ...d, visible: !d.visible } : d;
    });

    setRoutes(newData);
  };

  const handleExagerationChange = (e: ChangeEvent<HTMLInputElement>) => {
    setExageration(e.target.valueAsNumber);
  };

  const handleColorChange = debounce((e: ChangeEvent<HTMLInputElement>, index: number) => {
    const newColor = e.target.value;

    const newData = routes.map((d) => {
      return d.index === index ? { ...d, color: newColor } : d;
    });

    setRoutes(newData);
  }, 50);

  return (
    <Stack
      direction={{
        lg: "row",
      }}
    >
      <SimpleGrid columns={2} gap={"4"} maxWidth={"sm"} alignContent={"start"}>
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
                    defaultValue={route.color}
                    onChange={(e) => handleColorChange(e, route.index)}
                  />
                  <PrimaryButton onClick={() => handleVisibilityChange(route)}>
                    {route.visible ? "Hide" : "Show"}
                  </PrimaryButton>
                </Stack>
              )}
            </For>
          </Stack>
        </GridItem>

        <Input
          type="range"
          value={exageration}
          onChange={handleExagerationChange}
          min={0.1}
          max={2}
          step={0.01}
        />
      </SimpleGrid>
    </Stack>
  );
}
