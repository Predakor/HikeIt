import { Box, Flex, For, Tabs } from "@chakra-ui/react";
import { worstStatus } from "../model/worstStatus";
import type { BlockStatus } from "../shared/model/bench.types";
import { statusBadge } from "../shared/model/statusBadge";
import { benchTabs, type BenchTab, type BenchTabContext } from "./tabsConfig";

interface BenchTabsProps {
  view: string;
  context: BenchTabContext;
  onViewChange: (view: string) => void;
}

export function BenchTabs({ view, context, onViewChange }: BenchTabsProps) {
  const statusOf = ({ stage }: BenchTab) =>
    stage ? context.status[stage] : worstStatus(Object.values(context.status));

  return (
    <Tabs.Root
      lazyMount
      unmountOnExit
      value={view}
      onValueChange={(details) => onViewChange(details.value)}
    >
      <Tabs.List>
        <For each={benchTabs}>
          {(tab) => (
            <Tabs.Trigger key={tab.value} value={tab.value}>
              <TabLabel label={tab.label} status={statusOf(tab)} />
            </Tabs.Trigger>
          )}
        </For>
      </Tabs.List>

      <For each={benchTabs}>
        {(tab) => (
          <Tabs.Content key={tab.value} value={tab.value} px={0}>
            {tab.render(context)}
          </Tabs.Content>
        )}
      </For>
    </Tabs.Root>
  );
}

function TabLabel({ label, status }: { label: string; status: BlockStatus }) {
  return (
    <Flex align={"center"} gap={2}>
      <Box
        w={2}
        h={2}
        borderRadius={"full"}
        bg={`${statusBadge[status].colorPalette}.solid`}
        title={statusBadge[status].label}
      />
      {label}
    </Flex>
  );
}
