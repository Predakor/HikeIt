import type { FunctionComponent, ReactNode } from "react";

type ExtractObject<T> =
  T extends object ?
    T extends null ?
      never
    : T
  : never;

export type EntryType<TFor extends object> = EntryItem<keyof TFor> | EntryGroup<keyof TFor, TFor>;

export type TabConfig<TFor extends object> = EntryItem<keyof TFor>[];

export interface EntryItem<TKey> {
  type?: "item";
  key: TKey;
  label: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any -- every tab component takes its own data shape
  Component?: FunctionComponent<{ data: any }>;
}

export interface EntryGroup<TKey extends keyof TFor, TFor extends object> {
  type: "group";
  label: string;
  base: TKey;
  items: TabConfig<ExtractObject<TFor[TKey]>>;
  Wrapper?: FunctionComponent<{ children: ReactNode }>;
  dataGetter?: (data: TFor) => string;
}
