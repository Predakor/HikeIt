import type { ComparisonTarget } from "@/modules/PipelineBench/shared/types/PipelineConfig";
import { Button, Checkbox, Flex, Heading, SegmentGroup, Text } from "@chakra-ui/react";

const targetItems: { value: ComparisonTarget; label: string }[] = [
  { value: "currentProd", label: "Current config" },
  { value: "raw", label: "Raw file" },
];

interface BenchHeaderProps {
  target: ComparisonTarget;
  onTargetChange: (target: ComparisonTarget) => void;
  includeSteps: boolean;
  onIncludeStepsChange: (include: boolean) => void;
  onRunAll: () => void;
}

export function BenchHeader({
  target,
  onTargetChange,
  includeSteps,
  onIncludeStepsChange,
  onRunAll,
}: BenchHeaderProps) {
  return (
    <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
      <Heading size={"md"}>Pipeline bench</Heading>

      <Flex align={"center"} gap={3} wrap={"wrap"}>
        <Text as="span" fontSize={"sm"} color={"fg.muted"}>
          Compare to:
        </Text>
        <SegmentGroup.Root
          size={"sm"}
          value={target}
          onValueChange={(details) => {
            if (details.value === null) {
              return;
            }

            onTargetChange(details.value as ComparisonTarget);
          }}
        >
          <SegmentGroup.Indicator />
          <SegmentGroup.Items items={targetItems} />
        </SegmentGroup.Root>

        <Checkbox.Root
          size={"sm"}
          checked={includeSteps}
          onCheckedChange={(details) => onIncludeStepsChange(details.checked === true)}
        >
          <Checkbox.HiddenInput />
          <Checkbox.Control />
          <Checkbox.Label>Filter timings</Checkbox.Label>
        </Checkbox.Root>

        <Button size={"sm"} variant={"outline"} onClick={onRunAll}>
          Run all stale
        </Button>
      </Flex>
    </Flex>
  );
}
