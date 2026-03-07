import { Show, Span } from "@chakra-ui/react";
import type { Item, Props } from "../BarGraph";

export type BaseProps = {
  item: Item;
};
export function BarValue<T>({
  item,
  unit,
  formatValue = (t: any) => t,
}: BaseProps & Pick<Props<T>, "formatValue" | "unit">) {
  return (
    <Span fontSize={"sm"} color={"gray.300"}>
      {formatValue(item.value as any)}

      <Show when={unit}>{unit}</Show>
    </Span>
  );
}
