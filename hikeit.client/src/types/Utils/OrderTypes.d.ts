import type { FunctionComponent, ReactElement } from "react";
import type { ReactNode } from "react";
// ---------------------------
// Base tab config types
// ---------------------------

type ExtractObject<T> = T extends object ? (T extends null ? never : T) : never;

type EntryType<TFor> =
  | EntryItem<keyof TFor, TFor>
  | EntryGroup<keyof TFor, TFor>;

export type TabConfig<TFor extends object> = EntryType<TFor>[];

interface EntryItem<TKey, TFor> {
  type?: "item";
  key: TKey;
  label: string;
  Component?: FunctionComponent<{ data: any }>;
}

interface EntryGroup<TKey, TFor> {
  type: "group";
  label: string;
  base: TKey;
  items: TabConfig<ExtracObjcet<TFor[TKey]>>;
  Wrapper?: FunctionComponent<{ children: ReactNode }>;
  dataGetter?: (data: TFor) => string;
}

// ---------------------------
// Order config union
// ---------------------------

export type OrderConfig<TFor extends object> = OrderEntry<TFor>[];

export type OrderEntry<TFor extends object> =
  | OrderEntryItem<keyof TFor, TFor>
  | OrderEntryGroup<keyof TFor, TFor>
  | OrderEntryData<TFor>;

// ---------------------------
// Variants of OrderEntry
// ---------------------------

export interface OrderEntryItem<TKey, TFor> {
  type?: "item";
  key: TKey;
  Icon?: IconType;
  data?: TFor;
}

export interface OrderEntryGroup<TKey, TFor> {
  type: "group";
  label: string;
  items: OrderConfig<TFor>;
  base?: TKey;
  Wrapper?: FunctionComponent<{ children: ReactNode }>;
  RenderWith?: FunctionComponent<any>;
  dataGetter?: (data: TFor) => string;
}

export interface OrderEntryData<TFor> {
  type: "data";
  label: string;
  getData: (data: TFor) => string;
}
