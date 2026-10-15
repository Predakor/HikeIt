import useUser from "@/hooks/Auth/useUser";
import IsAdminUser from "@/Utils/IsAdminUser";
import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import type { BasicAnalytics } from "@/types/Api/analytics.types";
import { For, Stack, Tabs } from "@chakra-ui/react";
import { tripAnalyticTabs } from "../../Data/tabOrder";
import PipelineBench from "../PipelineBench/PipelineBench";

const pipelineBenchTabValue = "pipelineBench";

export function TripDetailsMenu({ data }: { data: BasicAnalytics }) {
  const config = tripAnalyticTabs;
  const { data: user } = useUser();
  // Hidden while the user is still loading, so the admin tab never flashes for non-admins.
  const isAdminUser = !!user && IsAdminUser(user);

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
        {isAdminUser ?
          <Tabs.Trigger value={pipelineBenchTabValue}>Pipeline bench</Tabs.Trigger>
        : null}
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
          {isAdminUser ?
            <Tabs.Content value={pipelineBenchTabValue}>
              <PipelineBench />
            </Tabs.Content>
          : null}
        </Stack>
      </Tabs.ContentGroup>
    </Tabs.Root>
  );
}
