import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type {
  PeakSummaryData,
  PeaksAnalytics,
} from "@/types/Api/analytics.types";
import type { ResourceUrl } from "@/types/Api/types";
import { GridItem, Show, SimpleGrid, Stack } from "@chakra-ui/react";
import { PeakBadge } from "../Common/PeakBadge";
import { RankStat } from "../Common/RankStat";
import { HighestPeak } from "./HighestPeak";
import { PeakSummary } from "./PeakSummary";
import ReachedPeaksList from "./ReachedPeaksList";
import extraPeaksData from "./extraPeaksData";

export default function PeaksAnalytics({ data }: { data: ResourceUrl }) {
  const getAnalytics = useResourceLink<PeaksAnalytics>(data);
  return (
    <FetchWrapper request={getAnalytics}>
      {(d) => <Analytics data={d} />}
    </FetchWrapper>
  );
}

function Analytics({ data }: { data: PeaksAnalytics }) {
  const { peaks, highest } = extraPeaksData(data);

  const secondarySpace = "-4 / span 3 ";

  return (
    <SimpleGrid
      display={{ base: "flex", lg: "grid" }}
      flexFlow={"column"}
      gridTemplateRows={"repeat(5,1fr)"}
      gridTemplateColumns={"repeat(7,1fr)"}
      gap={4}
    >
      <GridItem gridRow={"4 / span 2"} gridColumn={secondarySpace}>
        <SimpleCard title="Peaks Summary">
          <PeakSummary summary={data as unknown as PeakSummaryData} />
        </SimpleCard>
      </GridItem>

      <GridItem gridRow={"1 / span 3"} gridColumn={secondarySpace}>
        <SimpleCard
          title="Highest Peak"
          headerCta={<Reached reached={!highest.firstTime} />}
          bodyStyles={{ gap: 16 }}
        >
          <HighestPeak highest={highest} />
          <PeakRanking
            regionRank={{ place: 3, from: 5 }}
            userRank={{ place: 4, from: 6 }}
          />
        </SimpleCard>
      </GridItem>

      <GridItem gridRow={"1/6"} gridColumn={"1 / span 4"}>
        <SimpleCard title={"Reached Peaks"} bodyStyles={{ justify: "start" }}>
          <ReachedPeaksList peaks={peaks} />
        </SimpleCard>
      </GridItem>
    </SimpleGrid>
  );
}

function Reached({ reached }: { reached: boolean }) {
  return (
    <Show when={reached}>
      <PeakBadge color="yellow" text={"Reached for the first time"} />
    </Show>
  );
}

type Rank = {
  place: number;
  from: number;
};

interface Props {
  regionRank: Rank;
  userRank: Rank;
}

function PeakRanking({ regionRank, userRank }: Props) {
  return (
    <Stack justifyItems={"start"} direction={"row"}>
      <RankStat
        value={regionRank.place}
        maxValue={regionRank.from}
        label={"in the region"}
      />
      <RankStat
        value={userRank.place}
        maxValue={userRank.from}
        label={"in your history"}
      />
    </Stack>
  );
}
