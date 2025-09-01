import type { ReachedPeakWithBadges } from "@/components/ui/Inputs/File/trip.types";
import ReachedPeak from "@/components/Peaks/ReachedPeak/ReachedPeak";
import { Flex, For } from "@chakra-ui/react";

interface Props {
  peaks: ReachedPeakWithBadges[];
}

export default function ReachedPeaksList({ peaks }: Props) {
  return (
    <For each={peaks}>
      {(peak) => (
        <Flex alignItems={"center"} gapX={12}>
          <ReachedPeak peak={peak} />
        </Flex>
      )}
    </For>
  );
}
