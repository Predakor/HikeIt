import { Flex, For, Heading, Show, Stack, Text } from "@chakra-ui/react";
import { PeakBadge } from "../Common/PeakBadge";
import { PeakIcon } from "../Common/PeakIcon";
import type { ReachedPeakWithBadges } from "@/components/AddFile/AddFile/tripTypes";

interface Props {
  peaks: ReachedPeakWithBadges[];
}

export default function ReachedPeaksList({ peaks }: Props) {
  return (
    <For each={peaks}>
      {(peak) => (
        <Flex alignItems={"center"} gapX={12}>
          <ReachedPeakListItem peak={peak} />
        </Flex>
      )}
    </For>
  );
}

function ReachedPeakListItem({ peak }: { peak: ReachedPeakWithBadges }) {
  const { name, height, firstTime, reachedAt } = peak;
  const __mockupDistance__ = (Math.random() * 2000).toFixed(0);
  const __mockupTotalDistance__ = (Math.random() * 20000).toFixed(0);

  return (
    <>
      <Flex alignItems={"center"} gapX={4}>
        <PeakIcon />
        <Stack>
          <Heading fontSize={"2xl"}>{name}</Heading>
          <Text color={"fg.muted"}>{height}m</Text>
        </Stack>
      </Flex>
      <Show when={reachedAt}>
        <Stack>
          <Heading fontSize={"xl"}>8:50</Heading>
          <Text truncate hideBelow={"lg"} color={"fg.muted"}>
            {reachedAt}
          </Text>
        </Stack>
      </Show>
      {/* show when there is a distance markup */}
      <Show when={reachedAt}>
        <Stack hideBelow={"lg"}>
          <Heading fontSize={"xl"}>+{__mockupDistance__}m</Heading>
          <Text color={"fg.muted"}>{__mockupTotalDistance__}m</Text>
        </Stack>
      </Show>
      {firstTime && <PeakBadge text={"First time reached"} />}
    </>
  );
}
