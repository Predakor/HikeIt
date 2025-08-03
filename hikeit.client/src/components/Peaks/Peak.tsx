import { Flex, Stack, Heading, Text } from "@chakra-ui/react";
import { PeakIcon } from "../Trip/TripDetails/Common/PeakIcon";
import type { Peak } from "@/types/ApiTypes/Analytics";

export default function Peak({ peak }: { peak: Peak }) {
  return (
    <Flex alignItems={"center"} gapX={4}>
      <PeakIcon />
      <Stack>
        <Heading fontSize={"2xl"}>{peak.name}</Heading>
        <Text color={"fg.muted"}>{peak.height}m</Text>
      </Stack>
    </Flex>
  );
}
