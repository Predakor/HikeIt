import Peak from "@/components/Peaks/Peak";
import { PeakBadge } from "@/components/Trip/Details/Common/PeakBadge";
import type { PeakWithReachStatus } from "@/types/Api/peak.types";
import { Card, Stack, For, Flex } from "@chakra-ui/react";

interface Props {
  peaks: PeakWithReachStatus[];
}

export default function RegionPeaks({ peaks }: Props) {
  return (
    <Card.Root h={"full"} gapY={4}>
      <Card.Header>
        <Card.Title
          fontSize={{
            base: "2xl",
            lg: "3xl",
          }}
        >
          {"List of peaks in the region"}
        </Card.Title>
      </Card.Header>
      <Card.Body>
        <Stack gapY={8}>
          <PeakList peaks={peaks} />
        </Stack>
      </Card.Body>
    </Card.Root>
  );
}

function PeakList({ peaks }: { peaks: PeakWithReachStatus[] }) {
  return (
    <For each={peaks}>
      {(peak) => (
        <Flex
          color={peak.reached ? "" : "fg.muted"}
          alignItems={"center"}
          gapX={12}
          key={peak.id}
        >
          <Peak peak={peak} />
          {peak.reached && <PeakBadge color="green" text={"Reached"} />}
        </Flex>
      )}
    </For>
  );
}
