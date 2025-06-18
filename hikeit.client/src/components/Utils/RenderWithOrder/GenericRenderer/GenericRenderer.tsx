import type {
  OrderEntry,
  OrderEntryData,
  OrderEntryGroup,
  OrderEntryItem,
} from "@/types/Utils/OrderTypes";
import type { ReactElement } from "react";
import { GroupRenderer } from "../GroupRenderer/GroupRenderer";
import { ItemRenderer } from "../ItemRenderer/ItemRenderer";

type Handlers<T> = {
  entry: (entry: OrderEntryItem<keyof T, T>) => ReactElement;
  group: (entry: OrderEntryGroup<keyof T, T>) => ReactElement;
  data: (entry: OrderEntryData<T>) => ReactElement;
};

function matchEntry<T extends object>(
  entry: OrderEntry<T>,
  handlers: Handlers<T>
) {
  if (entry.type === "group") return handlers.group(entry);
  if (entry.type === "data") return handlers.data(entry);
  return handlers.entry(entry);
}

interface GenericRendererProps<T extends object> {
  entry: OrderEntry<T>;
  data: T;
}

export default function GenericRenderer<T extends object>({
  entry,
  data,
}: GenericRendererProps<T>) {
  return matchEntry(entry, {
    entry: (e) => (
      <ItemRenderer entry={e} data={data[e.key]} key={e.key as string} />
    ),
    group: (g) => <GroupRenderer group={g} data={data} key={g.label} />,
    data: (d) => <p key={"some-unique"}></p>,
  });
}
