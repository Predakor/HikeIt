import { Heading, Show, Span, Stack, Strong, Text } from "@chakra-ui/react";

interface Props {
  title: string;
  helperText?: string;
  stat: number;
  unit: {
    name: string;
    ratio?: number;
  };
  options?: {
    valueIn: "raw" | "percentile" | "rounded";
  };
}

export function ComparedStat({
  title,
  helperText,
  stat,
  unit,
  options,
}: Props) {
  const ratio = unit?.ratio ?? 1;
  const rawValue = stat / ratio;
  const valueInUnits = transformers[options?.valueIn || "rounded"](rawValue);
  return (
    <Stack gapY={0}>
      <Heading as={"h4"} color={"gray.400"} fontSize={"2xl"}>
        {`${title} `}
        <Strong color={"gray.100"}>{valueInUnits}</Strong>
        <Span fontSize={"smaller"}> {unit.name}</Span>
      </Heading>
      <Show when={helperText}>
        <Text color={"gray.400"} fontSize={"sm"} lineHeight={"1"}>
          {helperText}
        </Text>
      </Show>
    </Stack>
  );
}

const transformers = {
  raw: (t: number) => t,
  percentile: (t: number) => (t * 100).toFixed(1) + "%",
  rounded: (t: number) => Math.round(t),
};
