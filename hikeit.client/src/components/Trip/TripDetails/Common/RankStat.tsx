import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { Span, Stat } from "@chakra-ui/react";

interface Props {
  value: string | number;
  maxValue: string | number;
  label: string;
}

export function RankStat({ value, maxValue, label }: Props) {
  return (
    <Stat.Root alignItems={"center"} gapY={2}>
      <Stat.ValueText alignItems={"baseline"} fontSize={"4xl"}>
        <Span fontSize={"3xl"}>#</Span>
        {value}
        <Stat.ValueUnit fontSize={"lg"}>/{maxValue}</Stat.ValueUnit>
      </Stat.ValueText>
      <Stat.Label fontSize={"md"}>{KeyToLabelFormatter(label)}</Stat.Label>
    </Stat.Root>
  );
}
