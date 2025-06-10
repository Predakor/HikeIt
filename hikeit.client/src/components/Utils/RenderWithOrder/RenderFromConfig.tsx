import { For } from "@chakra-ui/react";
import RenderEntry from "./GenericRenderer/GenericRenderer";
import type { OrderConfig } from "@/types/Utils/OrderTypes";

export interface RenderOrderConfig<TFor extends object> {
  data: TFor;
  config: OrderConfig<TFor>;
}

export default function RenderFromConfig<T extends object>(
  props: RenderOrderConfig<T>
) {
  const { data, config } = props;

  return (
    <For
      each={config}
      children={(entry, i) => <RenderEntry entry={entry} data={data} key={i} />}
    />
  );
}
