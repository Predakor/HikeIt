import type { FunctionComponent, ReactElement } from "react";
import type { ReactNode } from "react";
// ---------------------------
// Base tab config types
// ---------------------------

export type TabConfig<TFor extends object> = TabEntry<keyof TFor>[];

export interface TabEntry<TKey> {
  key: TKey;
  label: string;
  Icon?: IconType;
  Component?: FunctionComponent<{ data: any }>;
}

// ---------------------------
// Order config union
// ---------------------------

export type OrderConfig<TFor extends object> = OrderEntry<TFor>[];

export type OrderEntry<TFor> =
  | OrderEntryItem<keyof TFor, TFor>
  | OrderEntryGroup<TFor>
  | OrderEntryData<TFor>;

// ---------------------------
// Variants of OrderEntry
// ---------------------------

export interface OrderEntryItem<TKey, TFor> extends TabEntry<TKey> {
  type?: "entry";
  key: TKey;
  label: string;
  Icon?: IconType;
  Component?: FunctionComponent<{ data: any }>;
  data?: TFor;
}

export interface OrderEntryGroup<TFor> {
  type: "group";
  label: string;
  items: OrderConfig<TFor>;
  Wrapper?: FunctionComponent<{ children: ReactNode }>;
  dataGetter?: (data: TFor) => string;
}

export interface OrderEntryData<TFor> {
  type: "data";
  label: string;
  getData: (data: TFor) => string;
}
