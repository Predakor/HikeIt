import type { LazyPage } from "@/data/routes/route.types";
import { lazy, type ComponentType } from "react";

export function lazyWithPreload(factory: () => Promise<{ default: ComponentType }>): LazyPage {
  return Object.assign(lazy(factory), { preload: factory });
}
