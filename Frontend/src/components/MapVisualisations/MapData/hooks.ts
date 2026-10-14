import { requireContext } from "@/Utils/requireContext";
import { createContext, useContext, useSyncExternalStore } from "react";
import type { HoverStore } from "./hoverStore";
import type { HoverTarget, MapDataActions, MapDataGlobals, RouteData } from "./mapData.types";

// contexts live here (not in the provider file) so the .tsx only exports a component
export const MapRoutesContext = createContext<RouteData[] | undefined>(undefined);
export const MapGlobalsContext = createContext<MapDataGlobals | undefined>(undefined);
export const MapActionsContext = createContext<MapDataActions | undefined>(undefined);
export const HoverStoreContext = createContext<HoverStore | undefined>(undefined);

const providerName = "MapDataProvider";

export function useMapRoutes(): RouteData[] {
  return requireContext(useContext(MapRoutesContext), "useMapRoutes", providerName);
}

export function useMapGlobals(): MapDataGlobals {
  return requireContext(useContext(MapGlobalsContext), "useMapGlobals", providerName);
}

export function useMapActions(): MapDataActions {
  return requireContext(useContext(MapActionsContext), "useMapActions", providerName);
}

function useHoverStore(): HoverStore {
  return requireContext(useContext(HoverStoreContext), "useHoverStore", providerName);
}

export function useIsHovered(kind: HoverTarget["kind"], id: number): boolean {
  const store = useHoverStore();
  return useSyncExternalStore(store.subscribe, () => {
    const hovered = store.get();
    return hovered?.kind === kind && hovered.id === id;
  });
}

// pass a kind to ignore hovers of other kinds (e.g. a hovered peak must not dim the routes)
export function useHasHover(kind?: HoverTarget["kind"]): boolean {
  const store = useHoverStore();
  return useSyncExternalStore(store.subscribe, () => {
    const hovered = store.get();
    const isMatchingKind = kind === undefined || hovered?.kind === kind;

    return hovered !== undefined && isMatchingKind;
  });
}

export function useSetHovered(): (target: HoverTarget | undefined) => void {
  return useHoverStore().set;
}
