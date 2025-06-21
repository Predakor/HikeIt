import { dateOnlyToString } from "@/Utils/Formatters/dateFormats";
import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import type {
  ReachedPeak,
  ReachedPeak as ReachedPeakListItem,
} from "@/components/AddTripForm/AddTrip/tripTypes";
import RowStat from "@/components/Stats/RowStat";
import {
  Badge,
  Card,
  Flex,
  For,
  Heading,
  Icon,
  SimpleGrid,
  Stack,
  Stat,
  Text,
} from "@chakra-ui/react";
import { GiMountaintop } from "react-icons/gi";
import type { PeaksAnalytic } from "../../Types/TripAnalyticsTypes";
import GetStatsMeta from "@/Utils/GetStatsMeta";

interface ReachedPeakWithBadges extends ReachedPeak {
  isHighest: boolean;
}

export default function PeaksAnalytics({ data }: { data: PeaksAnalytic }) {
  let highest = data.reached.reduce((max, item) =>
    item.height > max.height ? item : max
  );

  const peaks: ReachedPeakWithBadges[] = data.reached
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .map((p) => ({
      ...p,
      reachedAt: dateOnlyToString(new Date().toISOString().split("T")[0]),
      isHighest: p.height === highest.height,
    }));

  highest = {
    ...highest,
    reachedAt: dateOnlyToString(new Date().toISOString().split("T")[0]),
  };
  const secondarySpace = "-4 / span 4 ";

  return (
    <SimpleGrid
      gridTemplateRows={{ base: "", lg: "repeat(4,1rf)" }}
      gridTemplateColumns={{ base: "", lg: "repeat(7,1fr)" }}
      gap={8}
    >
      <Card.Root gridRow={"2/5"} gridColumn={secondarySpace}>
        <Card.Header>
          <PeakCardTitle title={"Peaks Summary"} />
        </Card.Header>
        <Card.Body>
          <Stack>
            <PeakStat value={5} label={"Total"} />
            <PeakStat value={3} label={"Unique"} />
            <PeakStat value={1} label={"First"} />
          </Stack>
        </Card.Body>
      </Card.Root>

      <Card.Root gridRow={"1"} gridColumn={secondarySpace}>
        <Card.Header
          display={"flex"}
          justifyContent={"space-between"}
          flexFlow={"row"}
        >
          <PeakCardTitle title={"Highest Peak"} />
          {!highest.firstTime && (
            <PeakBadge color="green" text={"Reached for the first time"} />
          )}
        </Card.Header>
        <Card.Body justifyContent={"center"}>
          <Flex alignItems={"center"} gapX={8}>
            <Icon fontSize={52}>
              <GiMountaintop />
            </Icon>

            <RowStat value={highest.name} label={"name"} />

            <RowStat
              value={highest.height}
              addons={{ unit: "m" }}
              label={"height"}
            />

            {highest.reachedAt && (
              <RowStat value={highest.reachedAt} label={"reached at"} />
            )}
          </Flex>
        </Card.Body>
      </Card.Root>

      <Card.Root gridRow={"1/5"} gridColumn={"1 / span 4"}>
        <Card.Header>
          <PeakCardTitle title={"Reached Peaks"} />
        </Card.Header>
        <Card.Body>
          <Stack gapY={6}>
            <For each={peaks}>
              {(peak) => (
                <Flex alignItems={"center"} gapX={12}>
                  <ReachedPeakListItem peak={peak} />
                </Flex>
              )}
            </For>
          </Stack>
        </Card.Body>
      </Card.Root>
    </SimpleGrid>
  );
}

function PeakStat({ value, label }: { value: string | number; label: string }) {
  return (
    <Stat.Root>
      <Flex alignItems={"baseline"} gapX={4}>
        <Stat.Label>{label}</Stat.Label>
        <Stat.ValueText>{value}</Stat.ValueText>
      </Flex>
    </Stat.Root>
  );
}

function ReachedPeakListItem({ peak }: { peak: ReachedPeakWithBadges }) {
  const { name, height, firstTime, isHighest, reachedAt } = peak;
  const __mockupDistance__ = (Math.random() * 2000).toFixed(0);
  const __mockupTotalDistance__ = (Math.random() * 20000).toFixed(0);

  return (
    <>
      <Flex alignItems={"center"} gapX={4}>
        <Icon fontSize={52}>
          <GiMountaintop />
        </Icon>

        <Stack>
          <Heading fontSize={"2xl"}>{name}</Heading>
          <Text color={"fg.muted"}>{height}m</Text>
        </Stack>
      </Flex>
      {reachedAt && (
        <Stack>
          <Heading fontSize={"xl"}>8:50</Heading>
          <Text color={"fg.muted"}>{reachedAt}</Text>
        </Stack>
      )}

      {reachedAt && (
        <Stack>
          <Heading fontSize={"xl"}>+{__mockupDistance__}m</Heading>
          <Text color={"fg.muted"}>{__mockupTotalDistance__}m</Text>
        </Stack>
      )}

      {isHighest && <PeakBadge text={"Highest"} color="green" />}
      {firstTime && <PeakBadge text={"First time reached"} />}
    </>
  );
}

interface Props {
  text: string;
  color?: "orange" | "yellow" | "green" | "teal" | "blue";
}

function PeakBadge({ text, color }: Props) {
  return (
    <Badge size={"lg"} colorPalette={color || "blue"}>
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
