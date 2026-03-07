import { Stack, Heading, Show, Text } from "@chakra-ui/react";

export default function ReachTime({ reachTime }: { reachTime?: Date }) {
  return (
    <Show when={reachTime}>
      <Stack>
        <Heading fontSize={"xl"}>8:50</Heading>
        <Text truncate hideBelow={"lg"} color={"fg.muted"}>
          {reachTime!.toDateString()}
        </Text>
      </Stack>
    </Show>
  );
}
