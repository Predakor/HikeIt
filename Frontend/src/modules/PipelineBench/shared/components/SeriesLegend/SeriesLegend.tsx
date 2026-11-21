import { Box, Flex, For, Text } from "@chakra-ui/react";

interface LegendItem {
  label: string;
  color: string;
}

interface SeriesLegendProps {
  items: LegendItem[];
}

export function SeriesLegend({ items }: SeriesLegendProps) {
  return (
    <Flex align={"center"} gap={4} fontSize={"xs"} color={"fg.muted"}>
      <For each={items}>
        {(item) => (
          <Flex key={item.label} align={"center"} gap={1.5}>
            <Box w={2} h={2} borderRadius={"full"} bg={item.color} flexShrink={0} />
            <Text as={"span"}>{item.label}</Text>
          </Flex>
        )}
      </For>
    </Flex>
  );
}
