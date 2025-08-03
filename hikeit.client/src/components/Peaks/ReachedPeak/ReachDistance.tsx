import { Stack, Heading, Show, Text } from "@chakra-ui/react";

interface Props {
  reachDistance?: number;
}

export default function ReachDistance({ reachDistance }: Props) {
  return (
    <Show when={reachDistance}>
      <Stack hideBelow={"lg"}>
        <Heading fontSize={"xl"}>+{reachDistance}m</Heading>
        <Text color={"fg.muted"}>{reachDistance}m</Text>
      </Stack>
    </Show>
  );
}
