import type { HoverTarget } from "./mapData.types";

export interface HoverStore {
  get: () => HoverTarget | undefined;
  set: (target: HoverTarget | undefined) => void;
  subscribe: (listener: () => void) => () => void;
}

// external store so hover changes only notify subscribers, not the whole provider tree
export function createHoverStore(): HoverStore {
  let current: HoverTarget | undefined;
  const listeners = new Set<() => void>();

  return {
    get: () => current,
    set: (target) => {
      const isTarget = current?.kind === target?.kind && current?.id === target?.id;
      if (isTarget) {
        return;
      }
      current = target;
      listeners.forEach((listener) => listener());
    },
    subscribe: (listener) => {
      listeners.add(listener);
      return () => {
        listeners.delete(listener);
      };
    },
  };
}
