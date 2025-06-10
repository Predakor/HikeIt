import type { EntryGroup, EntryType } from "@/types/Utils/OrderTypes";
import type { BaseProps } from "./typesRender";
import RenderTabEntry from "./RenderTabEntry";

interface GroupRenderProps<T> extends BaseProps<T> {
  entry: EntryGroup<keyof T, T>;
}

export function GroupRender<T extends object>(props: GroupRenderProps<T>) {
  const { data, entry } = props;
  const { base, items } = entry;

  const dataBase = data[base] || data;

  return items.map((e, index) => (
    <RenderTabEntry
      {...props}
      data={dataBase}
      entry={e as EntryType<T>}
      key={index}
    />
  ));
}
