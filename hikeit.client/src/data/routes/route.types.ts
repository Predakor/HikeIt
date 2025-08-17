import type { ReactNode } from "react";

export type RouteEntry = RouteItem | RouteGroup;

export interface RouteItem {
  type: "item";
  path: string;
  label: string;
  Page: LazyPage;
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

export type LazyPage = React.LazyExoticComponent<React.ComponentType<any>> & {
  preload: () => void;
};
