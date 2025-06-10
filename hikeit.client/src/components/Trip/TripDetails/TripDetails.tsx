import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { EntryItem, EntryType } from "@/types/Utils/OrderTypes";
import { For, Heading, Tabs, VStack } from "@chakra-ui/react";
import type { ReactElement } from "react";
import { tripDetailsTabs } from "../Data/tabOrder";

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

interface Props<T> {
  entry: EntryType<T>;
  data: any;
  Renderer?: (entry: EntryItem<keyof T, T>, data: any) => ReactElement;
}

function RenderTabEntry<T extends object>(props: Props<T>) {
  const { entry, data, Renderer } = props;

  if (entry.type == "group") {
    const base = entry.base;
    const dataBase = data[base] || data;

    return entry.items.map((e, index) => (
      <RenderTabEntry
        entry={e as EntryType<T>}
        data={dataBase}
        Renderer={Renderer}
        key={index}
      />
    ));
  }

  const { key, Component } = entry;
  const stringKey = key as string;
  const getData = data[stringKey];

  if (Renderer) {
    return Renderer(entry, getData);
  }

  if (Component) {
    return <Component data={getData} />;
  }

  return <p>did you forget do define item to render</p>;
}
