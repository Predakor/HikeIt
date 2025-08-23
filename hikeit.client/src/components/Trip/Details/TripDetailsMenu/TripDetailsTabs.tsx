import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { BasicAnalytics } from "@/types/ApiTypes/Analytics";
import type { TabConfig } from "@/types/Utils/order.types";
import { For, Stack, Tabs } from "@chakra-ui/react";
import { tripAnalyticTabs } from "../../Data/tabOrder";

export function TripDetailsMenu({ data }: { data: BasicAnalytics }) {
  return (
    <Tabs.Root lazyMount={true} defaultValue={"route"}>
      <Tabs.List
        display={"flex"}
        gapX={{ base: 12, lg: 4 }}
        maxW={"100vw"}
        overflowX={{ base: "scroll", lg: "clip" }}
      >
        <TabTriggers data={data} config={tripAnalyticTabs} />
        <Tabs.Indicator bg={"bg.emphasized"} rounded="12" />
      </Tabs.List>

      <Tabs.ContentGroup asChild gap={8}>
        <Stack w={"full"} paddingX={4} justifyItems={"center"} gap={8}>
          <TabContents data={data} config={tripAnalyticTabs} />
        </Stack>
      </Tabs.ContentGroup>
    </Tabs.Root>
  );
}

interface Props {
  data: BasicAnalytics;
  config: TabConfig<BasicAnalytics>;
}

function TabContents({ data, config }: Props) {
  return (
    <For each={config}>
      {({ key, Component }) => (
        <Tabs.Content value={key} key={key}>
          {Component ? (
            <Component data={data[key]} key={key} />
          ) : (
            `Please add a component for key: ${key}`
          )}
        </Tabs.Content>
      )}
    </For>
  );
}

function TabTriggers({ data, config }: Props) {
  return (
    <For each={config}>
      {({ key, label }) => (
        <Tabs.Trigger value={key} disabled={!data[key]} key={key}>
          {ToTitleCase(label || KeyToLabelFormatter(key))}
        </Tabs.Trigger>
      )}
    </For>
  );
}
