import { arrayUtils } from "@/Utils/arrayUtils";
import type { Func } from "@/types/Utils/func.types";
import type { UnitTypes } from "@/types/Utils/stat.types";
import type { SystemStyleObject } from "@chakra-ui/react";
import { Box, Flex, For, Show, Span, Stack } from "@chakra-ui/react";

type Item = {
  name: string;
  value: number;
  color: SystemStyleObject["color"];
};

interface Props {
  items: Item[];
  styles?: {};
  formatValue?: Func<number, number>;
  unit?: UnitTypes;
}

export function BarGraph({ items, formatValue, unit }: Props) {
  const itemSum = arrayUtils.sum(items, (item) => item.value);

  const geRatio = (item: Item) => (item.value / itemSum) * 100;
  return (
    <Flex width={"full"} minHeight={"8"}>
      <For each={items}>
        {(item) => (
          <Stack
            minWidth={"1"}
            width={`${geRatio(item)}%`}
            gapY={1}
            css={{
              "&:first-of-type > div": { borderLeftRadius: "0.125rem" },
              "&:last-of-type > div": { borderRightRadius: "0.125rem" },
            }}
          >
            <BarValue item={item} formatValue={formatValue} unit={unit} />
            <BarBody item={item} />
            <BarName item={item} />
          </Stack>
        )}
      </For>
    </Flex>
  );
}

type BaseProps = {
  item: Item;
};

function BarValue({
  item,
  unit,
  formatValue = (t) => t,
}: BaseProps & Pick<Props, "formatValue" | "unit">) {
  return (
    <Span fontSize={"sm"}>
      {formatValue(item.value)}

      <Show when={unit}>{unit}</Show>
    </Span>
  );
}

//1km 500e
//6km 1015

//1.3km 672e
//6.6km 1500e

function BarBody({ item }: BaseProps) {
  const color = (item.color ?? "blue") as string;
  return (
    <Box
      boxShadow={"2xl"}
      boxShadowColor={color}
      backgroundColor={color}
      flexGrow={1}
      minH={"8"}
      height={"100%"}
    />
  );
}

function BarName({ item }: BaseProps) {
  return (
    <Span truncate minH={"fit-content"} fontWeight={"medium"} fontSize={"sm"}>
      {item.name}
    </Span>
  );
}
