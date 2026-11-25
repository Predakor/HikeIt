import type { EntryItem } from "@/types/Utils/order.types";
import type { ReactElement } from "react";

export interface BaseProps<T> {
  data: Record<string, unknown>;
  Renderer?: (entry: EntryItem<keyof T>, data: unknown) => ReactElement;
}
