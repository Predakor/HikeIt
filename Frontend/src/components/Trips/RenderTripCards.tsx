import { IconArrowDown } from "@/Icons/Icons";
import usePagePreload from "@/hooks/Utils/usePagePreload";
import type { TripSummaries } from "@/types/Api/TripDtos";
import { Accordion, Flex, For, SimpleGrid, Spacer, Stack } from "@chakra-ui/react";
import { formatter } from "../User/Stats/Utils/formatter";
import SubTitle from "../ui/Titles/SubTitle";
import TripCard from "./Card/TripCard";
import sortTrips from "./sortTrips";

interface Props {
  trips: TripSummaries;
}

function RenderTripCards({ trips }: Props) {
  usePagePreload("trips/:tripId");
  const groupedByYears = sortTrips(trips);

  return (
    <Accordion.Root
      display={"flex"}
      flexDirection={"column"}
      gap={16}
      collapsible
      multiple
      defaultValue={groupedByYears.map((g) => g.year.toString())}
    >
      <For each={groupedByYears}>
        {({ year, trips, totalDistance, totalDuration }) => (
          <Accordion.Item key={year} value={year.toString()}>
            <Stack>
              <Flex>
                <SubTitle title={year.toString()} />
                <Spacer />
                <Accordion.ItemTrigger width={"min"} _open={{ rotate: "180deg" }}>
                  <IconArrowDown size={20} />
                </Accordion.ItemTrigger>
              </Flex>
              <Stack color={"fg.muted"} direction={"row"} align={"center"} gap={"4"}>
                <div>{formatter.toKm(totalDistance)} KM</div>
                <div>{totalDuration.toString()} </div>
              </Stack>
            </Stack>
            <Accordion.ItemContent>
              <Accordion.ItemBody asChild>
                <SimpleGrid
                  gridTemplateColumns={{ lg: "repeat(4,1fr)" }}
                  autoFlow={{ lg: "column" }}
                  gap={8}
                >
                  <For each={trips}>{(trip) => <TripCard data={trip} key={trip.id} />}</For>
                </SimpleGrid>
              </Accordion.ItemBody>
            </Accordion.ItemContent>
          </Accordion.Item>
        )}
      </For>
    </Accordion.Root>
  );
}

export default RenderTripCards;
