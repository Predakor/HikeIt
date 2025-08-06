import api from "@/Utils/Api/apiRequest";
import {
  GenericFormatter,
  KeyToLabelFormatter,
} from "@/Utils/Formatters/valueFormatter";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import RowStat from "@/components/Stats/RowStat";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import {
  For,
  Grid,
  Heading,
  SimpleGrid,
  Stack,
  VStack,
} from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

export type UserStats = {
  totals: Totals;
  locations: Locations;
  metas: Metas;
};

export type Totals = {
  totalDistanceMeters: number;
  totalAscentMeters: number;
  totalDescentMeters: number;
  totalDuration: string; // ISO duration or you can convert to number (seconds) if needed
  totalPeaks: number;
  totalTrips: number;
};

export type Locations = {
  uniquePeaks: number;
  regionsVisited: number;
};

export type Metas = {
  firstHikeDate: string | null; // ISO date string e.g. '2025-01-01'
  lastHikeDate: string | null;
  longestTripMeters: number;
};

function UserSummaryPage() {
  const stats = useQuery<UserStats>({
    queryKey: ["user-stats"],
    queryFn: () => api.get<UserStats>("users/stats"),
    staleTime: 1000 * 60 * 60,
  });

  return (
    <FetchWrapper request={stats}>
      {(data) => {
        const totals = ObjectToArray(data.totals);
        const locations = ObjectToArray(data.locations);
        const metas = ObjectToArray(data.metas);

        return (
          <SimpleGrid
            justifySelf={"center"}
            alignSelf={"center"}
            columns={2}
            gap={16}
            placeContent={"center"}
          >
            <RenderStats name="Totals" stats={totals} />

            <RenderStats name="Locations" stats={locations} />

            <RenderStats name="Meatas" stats={metas} />
          </SimpleGrid>
        );
      }}
    </FetchWrapper>
  );
}

function RenderStats({
  stats,
  name,
}: {
  stats: Array<[string, string | number]>;
  name: string;
}) {
  return (
    <VStack gapY={"4"}>
      <Heading fontSize={"3xl"} textAlign={"center"} as="h3">
        {name}
      </Heading>
      <SimpleGrid columns={{ base: 2, lg: 3 }} gap={8}>
        <For each={stats}>
          {([key, value]) => (
            <RowStat
              value={GenericFormatter(value)}
              label={KeyToLabelFormatter(key)}
            />
          )}
        </For>
      </SimpleGrid>
    </VStack>
  );
}

export default UserSummaryPage;
