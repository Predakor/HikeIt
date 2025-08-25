import { arrayUtils } from "@/Utils/arrayUtils";
import type { Func } from "@/types/Utils/func.types";
import type { UnitTypes } from "@/types/Utils/stat.types";
import type { SystemStyleObject } from "@chakra-ui/react";
import { Flex, For, Stack } from "@chakra-ui/react";
import Bar from "./Components";

export type Item = {
  name: string;
  value: number;
  color: SystemStyleObject["color"];
};

export interface Props<T> {
  items: Item[];
  styles?: {};
  formatValue?: Func<T, string | string>;
  unit?: UnitTypes;
}

export function BarGraph<T>({ items, formatValue, unit }: Props<T>) {
  const itemSum = arrayUtils.sum(items, (item) => item.value);

  const geRatio = (item: Item) => (item.value / itemSum) * 100;
  return (
    <Flex width={"full"} minHeight={"8"} gap={1}>
      <For each={items.filter((i) => i.value !== 0)}>
        {(item) => (
          <Stack minWidth={"1"} width={`${geRatio(item)}%`} gapY={1}>
            <Bar.Value item={item} formatValue={formatValue} unit={unit} />
            <Bar.Content item={item} />
            <Bar.Label item={item} />
          </Stack>
        )}
      </For>
    </Flex>
  );
}
