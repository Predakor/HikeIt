import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { EntryItem } from "@/types/Utils/OrderTypes";
import { For, Heading, Tabs, VStack } from "@chakra-ui/react";
import { tripDetailsTabs } from "../Data/tabOrder";
import RenderTabEntry from "@/components/Utils/RenderTabs/RenderTabEntry";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs;

  return (
    <VStack h={"full"} w={"full"} alignItems={"flex-start"} gap={"2em"}>
      <Heading fontSize={"4xl"}>{data.base.name}</Heading>
      <Tabs.Root
        lazyMount
        unmountOnExit
        orientation="vertical"
        defaultValue={"base"}
      >
        <Tabs.List alignSelf={{ base: "flex-start" }}>
          <For
            each={tabOrder}
            children={(entry) => (
              <RenderTabEntry entry={entry} data={data} Renderer={TabTrigger} />
            )}
          />

          <Tabs.Indicator bg={"bg.emphasized"} rounded="l2" />
        </Tabs.List>

        <Tabs.ContentGroup gap={8}>
          <For
            each={tabOrder}
            children={(entry) => (
              <RenderTabEntry entry={entry} data={data} Renderer={TabContent} />
            )}
          />
        </Tabs.ContentGroup>
      </Tabs.Root>
    </VStack>
  );
}
export default TripDetails;

function TabContent(
  entry: EntryItem<keyof TripDtoFull, TripDtoFull>,
  data: any
) {
  const { key, Component } = entry;
  return (
    <Tabs.Content value={key} key={key}>
      {Component ? (
        <Component data={data} key={key} />
      ) : (
        `Please add a component for key: ${key}`
      )}
    </Tabs.Content>
  );
}

function TabTrigger(
  entry: EntryItem<keyof TripDtoFull, TripDtoFull>,
  data: any
) {
  const { key } = entry;
  const label = entry.label || KeyToLabelFormatter(entry.key);

  return (
    <Tabs.Trigger value={key} disabled={!data} key={key}>
      {ToTitleCase(label)}
    </Tabs.Trigger>
  );
}
