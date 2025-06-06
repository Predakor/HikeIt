import { ToTitleCase } from "@/Utils/ObjectToArray";
import { For, HStack, Heading, Stack, Tabs, VStack } from "@chakra-ui/react";
import type { FunctionComponent } from "react";
import type { IconType } from "react-icons";
import type { TripDtoFull } from "../AddTripForm/AddTrip/types";
import Trip from "./Trip";
import TripAnalytics from "./TripAnalytics";
import TripGraph from "./TripGraph";

type TabEntry = {
  key: keyof TripDtoFull;
  label: string;
  Icon?: IconType;
  Component: FunctionComponent<{ data: any }>;
};

const tabOrder: TabEntry[] = [
  { key: "base", label: "Base", Component: Trip },
  { key: "trackAnalytic", label: "Analytics", Component: TripAnalytics },
  { key: "trackGraph", label: "Graph", Component: TripGraph },
  { key: "reachedPeaks", label: "Reached Peaks", Component: TripGraph },
  { key: "region", label: "Region", Component: TripGraph },
];

function TripDetails({ data }: { data: TripDtoFull }) {
  return (
    <VStack justifyItems={"center"} gap={"2em"}>
      <Heading>Trip summary for {data.id}</Heading>

      <Tabs.Root orientation="vertical" defaultValue={tabOrder[0].key}>
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
