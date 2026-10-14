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

export function useHasHover(): boolean {
  const store = useHoverStore();
  return useSyncExternalStore(store.subscribe, () => store.get() !== undefined);
}

export function useSetHovered(): (target: HoverTarget | undefined) => void {
  return useHoverStore().set;
}
