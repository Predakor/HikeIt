import type { FunctionComponent, ReactNode } from "react";

export type RouteEntry = RouteItem | RouteGroup;

export interface RouteItem {
  type: "item";
  path: string;
  label: string;
  Page: FunctionComponent;
  hidden?: boolean;
  Icon?: ReactNode; // optional icon, e.g. from react-icons or chakra icons
}

export interface RouteGroup {
  type: "group";
  path: string;
  label: string;
  pages: RouteItem[];
  hidden?: boolean;
}
