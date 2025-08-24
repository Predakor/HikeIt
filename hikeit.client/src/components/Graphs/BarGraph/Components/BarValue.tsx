import { Show, Span } from "@chakra-ui/react";
import type { Item, Props } from "../BarGraph";

export type BaseProps = {
  item: Item;
};
export function BarValue({
  item,
  unit,
  formatValue = (t) => t,
}: BaseProps & Pick<Props, "formatValue" | "unit">) {
  return (
    <Span fontSize={"sm"} color={"gray.300"}>
      {formatValue(item.value)}

      <Show when={unit}>{unit}</Show>
    </Span>
  );
}
