import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { Heading, Tabs, VStack } from "@chakra-ui/react";
import { tripDetailsTabs } from "../Data/tabOrder";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs;
  return (
    <VStack justifyItems={"center"} gap={"2em"}>
      <Heading>Trip summary for {data.id}</Heading>

      <Tabs.Root
        lazyMount
        unmountOnExit
        orientation="vertical"
        defaultValue={tabOrder[0].key}
      >
        <Tabs.List alignSelf={{ base: "flex-start" }}>
          {tabOrder.map(({ key, label }) => (
            <Tabs.Trigger value={key} disabled={!data[key]} key={key}>
              {ToTitleCase(label)}
            </Tabs.Trigger>
          ))}
          <Tabs.Indicator bg={"bg.emphasized"} rounded="l2" />
        </Tabs.List>

        <Tabs.ContentGroup gap={8}>
          {tabOrder.map(({ key, Component }) => (
            <Tabs.Content value={key} key={key}>
              <Component data={data[key]} />
            </Tabs.Content>
          ))}
        </Tabs.ContentGroup>
      </Tabs.Root>
    </VStack>
  );
}
export default TripDetails;
