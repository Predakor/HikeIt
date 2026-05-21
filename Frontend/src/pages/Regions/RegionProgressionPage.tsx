import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { ToTitleCase } from "@/Utils/ObjectToArray";
import RegionProgress from "@/components/UserRegions/Progression/RegionProgress";
import RegionTrips from "@/components/UserRegions/Trips/RegionTrips";
import RegionRoutesVisualization from "@/components/UserRegions/Visualizations/RegionRoutesVisualization";
import FetchWrapper from "@/components/Utils/Fetching";
import useUserRegion from "@/hooks/Regions/useRegionProgress";
import type { UserRegionData } from "@/types/Api/region.types";
import type { TabConfig } from "@/types/Utils/order.types";
import { For, Stack, Tabs } from "@chakra-ui/react";
import { useMemo, useEffect } from "react";
import { useSearchParams } from "react-router";

const tripAnalyticTabs: TabConfig<UserRegionData> = [
  { key: "progress", label: "", Component: RegionProgress },
  { key: "trips", label: "", Component: RegionTrips },
  { key: "visualizations", label: "", Component: RegionRoutesVisualization },
];

const tabMap = new Map(tripAnalyticTabs.map((x) => [x.key, x]));
const tabParamName = "tab";

export default function RegionProgressionsPage() {
  const config = tripAnalyticTabs;
  const getRegionProgress = useUserRegion();
  const [searchParams, setSearchParams] = useSearchParams();

  const activeTab = useMemo(() => {
    const tabParam = searchParams.get(tabParamName) as keyof UserRegionData | undefined;
    return tabParam && tabMap.has(tabParam) ? tabParam : "progress";
  }, [searchParams]);

  useEffect(() => {
    if (searchParams.get(tabParamName) !== activeTab) {
      const nextSearchParams = new URLSearchParams(searchParams);
      nextSearchParams.set(tabParamName, activeTab);
      setSearchParams(nextSearchParams, { replace: true });
    }
  }, [activeTab, searchParams, setSearchParams]);

  const setTab = (tab: Tabs.TabsValueChangeDetails) => {
    const nextSearchParams = new URLSearchParams(searchParams);
    nextSearchParams.set(tabParamName, tab.value);
    setSearchParams(nextSearchParams);
  };

  return (
    <FetchWrapper request={getRegionProgress}>
      {(data) => (
        <Tabs.Root lazyMount={true} value={activeTab} onValueChange={setTab}>
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
      )}
    </FetchWrapper>
  );
}
