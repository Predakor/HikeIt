import type { ByIndex } from "@/types/Utils/mapping.types";
import type { ReducerHandlers, SetAtIndex, SetAtIndexActions } from "@/types/Utils/reducer.types";

// value type of each per-route override; state and actions are both derived from this
type PerRouteValues = { visibility: boolean; width: number; color: string; gradient: boolean };
type PerRouteOverride = keyof PerRouteValues;

export type MapDataOverrides = ByIndex<PerRouteValues> & { globalGradient: boolean };

export type MapDataAction =
  | SetAtIndexActions<PerRouteValues>
  | { type: "globalGradient"; value?: boolean };

export const initialMapDataOverrides: MapDataOverrides = {
  visibility: {},
  width: {},
  color: {},
  gradient: {},
  globalGradient: false,
};

// the four per-route overrides all just set one entry of their record
const setAtIndex =
  <K extends PerRouteOverride>(key: K) =>
  (state: MapDataOverrides, action: SetAtIndex<PerRouteValues, K>) => ({
    ...state,
    [key]: { ...state[key], [action.index]: action.value },
  });

const handlers: ReducerHandlers<MapDataOverrides, MapDataAction> = {
  visibility: setAtIndex("visibility"),
  width: setAtIndex("width"),
  color: setAtIndex("color"),
  gradient: setAtIndex("gradient"),
  globalGradient: (state, action) => ({
    ...state,
    globalGradient: action.value ?? !state.globalGradient,
  }),
};

export function mapDataReducer(state: MapDataOverrides, action: MapDataAction): MapDataOverrides {
  // the lookup can't correlate action.type with its payload, so this one cast is needed
  const handle = handlers[action.type] as (
    state: MapDataOverrides,
    action: MapDataAction,
  ) => MapDataOverrides;

  return handle(state, action);
}
