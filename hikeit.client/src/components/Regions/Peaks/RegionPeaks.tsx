import Peak from "@/components/Peaks/Peak";
import { PeakBadge } from "@/components/Trip/Details/Common/PeakBadge";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { PeakWithReachStatus } from "@/types/Api/peak.types";
import { Flex, For } from "@chakra-ui/react";

interface Props {
  peaks: PeakWithReachStatus[];
}

export default function RegionPeaks({ peaks }: Props) {
  return (
    <SimpleCard title={"Peaks in the region"} bodyStyles={{ justify: "start" }}>
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
    </SimpleCard>
  );
}
