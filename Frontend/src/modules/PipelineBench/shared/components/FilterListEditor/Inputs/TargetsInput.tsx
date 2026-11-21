import { Button, Flex, For, Text } from "@chakra-ui/react";
import type { FilterFieldValue } from "../filterCatalog";

export interface PointFieldsField {
  kind: "pointFields";
  name: string;
  defaultValue: string;
}

type PointFieldName = "Latitude" | "Longitude" | "Elevation";

/** Flags an alias stands for, so a server-sent "Coordinates" or "All" still reads as individual flags. */
const pointFieldAliases: Record<string, PointFieldName[]> = {
  Latitude: ["Latitude"],
  Longitude: ["Longitude"],
  Elevation: ["Elevation"],
  Coordinates: ["Latitude", "Longitude"],
  All: ["Latitude", "Longitude", "Elevation"],
};

const pointFieldOrder: PointFieldName[] = ["Latitude", "Longitude", "Elevation"];

function parsePointFields(value: string): Set<PointFieldName> {
  const names = value
    .split(",")
    .map((name) => name.trim())
    .flatMap((name) => pointFieldAliases[name] ?? []);

  return new Set(names);
}

function formatPointFields(fields: Set<PointFieldName>): string {
  return pointFieldOrder.filter((name) => fields.has(name)).join(", ");
}

interface TargetsInputProps {
  field: PointFieldsField;
  value: FilterFieldValue | undefined;
  onValueChange: (value: string) => void;
}

const targetToggles: { field: PointFieldName; label: string; title: string }[] = [
  { field: "Latitude", label: "Lat", title: "Latitude" },
  { field: "Longitude", label: "Lon", title: "Longitude" },
  { field: "Elevation", label: "Ele", title: "Elevation" },
];

export function TargetsInput({ field, value, onValueChange }: TargetsInputProps) {
  const active = parsePointFields(String(value ?? field.defaultValue));

  const toggle = (target: PointFieldName) => {
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

    onValueChange(formatPointFields(next));
  };

  return (
    <Flex align={"center"} gap={1}>
      <Text fontSize={"xs"} color={"fg.muted"}>
        Targets
      </Text>
      <For each={targetToggles}>
        {({ field: target, label, title }) => (
          <Button
            key={target}
            size={"xs"}
            variant={active.has(target) ? "solid" : "outline"}
            aria-label={`Target ${title}`}
            aria-pressed={active.has(target)}
            title={title}
            onClick={() => toggle(target)}
          >
            {label}
          </Button>
        )}
      </For>
    </Flex>
  );
}
