import { Button, Flex, Heading, SegmentGroup, Text } from "@chakra-ui/react";

export type BaselineSource = "config" | "raw";

const baselineItems: { value: BaselineSource; label: string }[] = [
  { value: "config", label: "Current config" },
  { value: "raw", label: "Raw file" },
];

interface BenchHeaderProps {
  baselineSource: BaselineSource;
  onBaselineSourceChange: (source: BaselineSource) => void;
  onRunAll: () => void;
}

export function BenchHeader({
  baselineSource,
  onBaselineSourceChange,
  onRunAll,
}: BenchHeaderProps) {
  return (
    <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
      <Heading size={"md"}>Pipeline bench</Heading>

      <Flex align={"center"} gap={3} wrap={"wrap"}>
        <Text as="span" fontSize={"sm"} color={"fg.muted"}>
          Baseline:
        </Text>
        <SegmentGroup.Root
          size={"sm"}
          value={baselineSource}
          onValueChange={(details) => {
            if (details.value === null) {
              return;
            }

            onBaselineSourceChange(details.value as BaselineSource);
          }}
        >
          <SegmentGroup.Indicator />
          <SegmentGroup.Items items={baselineItems} />
        </SegmentGroup.Root>

        <Button size={"sm"} variant={"outline"} onClick={onRunAll}>
          Run all stale
        </Button>
      </Flex>
    </Flex>
  );
}
