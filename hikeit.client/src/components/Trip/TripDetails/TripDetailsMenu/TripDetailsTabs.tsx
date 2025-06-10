import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import type { EntryItem, TabConfig } from "@/types/Utils/OrderTypes";
import { For, Tabs } from "@chakra-ui/react";
import RenderTabEntry from "@/components/Utils/RenderTabs/RenderTabEntry";

interface Props {
  data: TripDtoFull;
  config: TabConfig<TripDtoFull>;
}
export function TripDetailsMenu({ data, config }: Props) {
  return (
    <Tabs.Root
      lazyMount
      unmountOnExit
      orientation="vertical"
      defaultValue={"base"}
      size={"lg"}
    >
      <Tabs.List
        alignSelf={{
          base: "flex-start",
        }}
      >
        <For
          each={config}
          children={(entry) => (
            <RenderTabEntry entry={entry} data={data} Renderer={TabTrigger} />
          )}
        />

        <Tabs.Indicator bg={"bg.emphasized"} rounded="l2" />
      </Tabs.List>

      <Tabs.ContentGroup gap={8}>
        <For
          each={config}
          children={(entry) => (
            <RenderTabEntry entry={entry} data={data} Renderer={TabContent} />
          )}
        />
      </Tabs.ContentGroup>
    </Tabs.Root>
  );
}
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
