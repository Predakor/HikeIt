import { Card, Show, SimpleGrid, Stack } from "@chakra-ui/react";
import { PeakBadge } from "../Common/PeakBadge";
import { RankStat } from "../Common/RankStat";
import { HighestPeak } from "./HighestPeak";
import { PeakSummary } from "./PeakSummary";
import ReachedPeaksList from "./ReachedPeaksList";
import usePeakAnalytics from "./usePeakAnalytics";
import type { PeaksAnalytics } from "@/types/ApiTypes/Analytics";

export default function PeaksAnalytics({ data }: { data: PeaksAnalytics }) {
  const { peaks, highest } = usePeakAnalytics(data);

  const secondarySpace = "-5 / span 4 ";

  return (
    <SimpleGrid
      display={{ base: "flex", lg: "grid" }}
      flexFlow={"column"}
      gridTemplateRows={"repeat(5,1fr)"}
      gridTemplateColumns={"repeat(7,1fr)"}
      gap={4}
    >
      <Card.Root gridRow={"4 / span 2"} gridColumn={secondarySpace}>
        <Card.Header>
          <PeakCardTitle title={"Peaks Summary"} />
        </Card.Header>
        <Card.Body justifyContent={"space-evenly"} gapY={8}>
          <PeakSummary summary={data.summary} />
        </Card.Body>
      </Card.Root>

      <Card.Root gridRow={"1 / span 3"} gridColumn={secondarySpace}>
        <Card.Header flexFlow={"row"} justifyContent={"space-between"}>
          <PeakCardTitle title={"Highest Peak"} />
          <Show when={!highest.firstTime}>
            <PeakBadge color="yellow" text={"Reached for the first time"} />
          </Show>
        </Card.Header>
        <Card.Body justifyContent={"space-around"} gapY={8}>
          <HighestPeak highest={highest} />
          <Stack justifyItems={"start"} direction={"row"}>
            <RankStat value={5} maxValue={23} label={"in the region"} />
            <RankStat value={7} maxValue={50} label={"in your history"} />
          </Stack>
        </Card.Body>
      </Card.Root>

      <Card.Root gridRow={"1/6"} gridColumn={"1 / span 3"}>
        <Card.Header>
          <PeakCardTitle title={"Reached Peaks"} />
        </Card.Header>
        <Card.Body gapY={8}>
          <ReachedPeaksList peaks={peaks} />
        </Card.Body>
      </Card.Root>
    </SimpleGrid>
  );
}

function PeakCardTitle({ title }: { title: string }) {
  return (
    <Card.Title color={"fg.muted"} fontSize={"2xl"}>
      {title}
    </Card.Title>
  );
}
