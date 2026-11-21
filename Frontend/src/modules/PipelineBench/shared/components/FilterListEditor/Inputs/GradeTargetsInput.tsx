import { Button, Flex, For, Text } from "@chakra-ui/react";
import type { FilterFieldValue } from "../filterCatalog";

export interface GradeTargetsField {
  kind: "gradeTargets";
  name: string;
  defaultValue: string;
}

type GradeTarget = "Ascent" | "Descent";

const gradeTargetOrder: GradeTarget[] = ["Ascent", "Descent"];

// "Both" is the server's alias for both flags.
const gradeTargetAliases: Record<string, GradeTarget[]> = {
  Ascent: ["Ascent"],
  Descent: ["Descent"],
  Both: ["Ascent", "Descent"],
};

function parseGradeTargets(value: string): Set<GradeTarget> {
  const names = value
    .split(",")
    .map((name) => name.trim())
    .flatMap((name) => gradeTargetAliases[name] ?? []);

  return new Set(names);
}

function formatGradeTargets(targets: Set<GradeTarget>): string {
  return gradeTargetOrder.filter((name) => targets.has(name)).join(", ");
}

interface GradeTargetsInputProps {
  field: GradeTargetsField;
  value: FilterFieldValue | undefined;
  onValueChange: (value: string) => void;
}

export function GradeTargetsInput({ field, value, onValueChange }: GradeTargetsInputProps) {
  const active = parseGradeTargets(String(value ?? field.defaultValue));

  const toggle = (target: GradeTarget) => {
    const next = new Set(active);

    if (next.has(target)) {
      next.delete(target);
    } else {
      next.add(target);
    }

    const wouldSelectNothing = next.size === 0;
    if (wouldSelectNothing) {
      return;
    }

    onValueChange(formatGradeTargets(next));
  };

  return (
    <Flex align={"center"} gap={1}>
      <Text fontSize={"xs"} color={"fg.muted"}>
        Targets
      </Text>
      <For each={gradeTargetOrder}>
        {(target) => (
          <Button
            key={target}
            size={"xs"}
            variant={active.has(target) ? "solid" : "outline"}
            aria-label={`Target ${target}`}
            aria-pressed={active.has(target)}
            onClick={() => toggle(target)}
          >
            {target}
          </Button>
        )}
      </For>
    </Flex>
  );
}
