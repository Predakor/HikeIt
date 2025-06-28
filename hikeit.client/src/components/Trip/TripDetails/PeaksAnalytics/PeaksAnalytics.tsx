import { dateOnlyToString } from "@/Utils/Formatters/dateFormats";
import type { ReachedPeak } from "@/components/AddTripForm/AddFile/tripTypes";
import RowStat from "@/components/Stats/RowStat";
import { BarSegment, useChart } from "@chakra-ui/charts";
import {
  Badge,
  Box,
  Card,
  Flex,
  Group,
  Icon,
  Show,
  SimpleGrid,
  Span,
  Stack,
  Stat,
} from "@chakra-ui/react";
import { GiMountaintop } from "react-icons/gi";
import type {
  PeakSummary,
  PeaksAnalytic,
} from "../../Types/TripAnalyticsTypes";
import { HighestPeak } from "./HighestPeak";
import ReachedPeaksList from "./ReachedPeaksList";
import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";

export interface ReachedPeakWithBadges extends ReachedPeak {
  isHighest: boolean;
}

export default function PeaksAnalytics({ data }: { data: PeaksAnalytic }) {
  let highest = data.reached.reduce((max, item) =>
    item.height > max.height ? item : max
  ) as ReachedPeakWithBadges;

  const peaks: ReachedPeakWithBadges[] = data.reached
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .map((p) => ({
      ...p,
      firstTime: Math.random() * 10 > 7,
      reachedAt: dateOnlyToString(new Date().toISOString().split("T")[0]),
      isHighest: Math.random() * 10 > 7,
    }));

  highest = {
    ...highest,
    isHighest: true,
    reachedAt: dateOnlyToString(new Date().toISOString().split("T")[0]),
  };

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

  function RankStat({
    value,
    maxValue,
    label,
  }: {
    value: string | number;
    maxValue: string | number;
    label: string;
  }) {
    return (
      <Stat.Root alignItems={"center"} gapY={2}>
        <Stat.ValueText alignItems={"baseline"} fontSize={"4xl"}>
          <Span fontSize={"3xl"}>#</Span>
          {value}
          <Stat.ValueUnit fontSize={"lg"}>/{maxValue}</Stat.ValueUnit>
        </Stat.ValueText>
        <Stat.Label fontSize={"md"}>{KeyToLabelFormatter(label)}</Stat.Label>
      </Stat.Root>
    );
  }
}

export function PeakIcon() {
  return (
    <Icon hideBelow={"lg"} fontSize={{ base: 32, md: 40, xl: 52 }}>
      <GiMountaintop />
    </Icon>
  );
}

interface Props {
  text: string;
  color?: "orange" | "yellow" | "green" | "teal" | "blue";
}

export function PeakBadge({ text, color }: Props) {
  return (
    <Badge
      size={{ base: "xs", lg: "lg" }}
      colorPalette={color || "blue"}
      truncate
    >
      {text}
    </Badge>
  );
}

function PeakCardTitle({ title }: { title: string }) {
  return (
    <Card.Title color={"fg.muted"} fontSize={"2xl"}>
      {title}
    </Card.Title>
  );
}

function PeakSummary({}: { summary: PeakSummary }) {
  const chart = useChart({
    sort: { by: "value", direction: "desc" },
    data: [
      { name: "Unique", value: 3, color: "purple.solid" },
      { name: "First Summits", value: 1, color: "orange" },
    ],
  });

  return (
    <>
      <Stack direction={{ base: "column", lg: "row" }}>
        <RowStat value={5} label={"total peaks"} />
        <RowStat value={3} label={"Unique peaks"} />
        <RowStat value={1} label={"First Summits"} />
      </Stack>
      <Box>
        <BarSegment.Root chart={chart}>
          <BarSegment.Content>
            <BarSegment.Value />
            <BarSegment.Bar />
            <BarSegment.Label />
          </BarSegment.Content>
        </BarSegment.Root>
      </Box>
    </>
  );
}
