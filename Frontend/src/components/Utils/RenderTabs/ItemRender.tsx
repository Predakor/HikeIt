import type { EntryItem } from "@/types/Utils/order.types";
import type { BaseProps } from "./typesRender";

interface ItemRenderProps<T> extends BaseProps<T> {
  entry: EntryItem<keyof T, T>;
}

export function ItemRender<T>({ entry, data, Renderer }: ItemRenderProps<T>) {
  const { key, Component } = entry;
  const stringKey = key as string;
  const getData = data[stringKey];

  if (Renderer) {
    return Renderer(entry, getData);
  }

  if (Component) {
    return <Component data={getData} />;
  }

  return <p>did you forget do define item to render</p>;
}
