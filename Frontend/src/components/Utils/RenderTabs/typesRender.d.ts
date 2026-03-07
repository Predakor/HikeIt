import type { EntryItem } from "@/types/Utils/order.types";
import type { ReactElement } from "react";

export interface BaseProps<T> {
  data: any;
  Renderer?: (entry: EntryItem<keyof T, T>, data: any) => ReactElement;
}
