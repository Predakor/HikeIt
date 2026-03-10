import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { BasicAnalytics } from "@/types/Api/analytics.types";
import { For, Stack, Tabs } from "@chakra-ui/react";
import { tripAnalyticTabs } from "../../Data/tabOrder";

export function TripDetailsMenu({ data }: { data: BasicAnalytics }) {
  const config = tripAnalyticTabs;
  return (
    <Tabs.Root lazyMount={true} defaultValue={"route"}>
      <Tabs.List display={"flex"} maxW={"100vw"} overflowX={{ base: "scroll", lg: "clip" }}>
        <For each={config}>
          {({ key, label }) => (
            <Tabs.Trigger value={key} disabled={false && !data[key]} key={key}>
              {ToTitleCase(label || KeyToLabelFormatter(key))}
            </Tabs.Trigger>
          )}
        </For>
        <Tabs.Indicator bg={"bg.emphasized"} rounded="12" />
      </Tabs.List>

      <Tabs.ContentGroup asChild gap={8} padding={0}>
        <Stack w={"full"} paddingX={4} justifyItems={"center"} gap={8}>
          <For each={config}>
            {({ key, Component }) => (
              <Tabs.Content value={key} key={key}>
                {Component ?
                  <Component data={data[key]} key={key} />
                : `Please add a component for key: ${key}`}
              </Tabs.Content>
            )}
          </For>
        </Stack>
      </Tabs.ContentGroup>
    </Tabs.Root>
  );
}
