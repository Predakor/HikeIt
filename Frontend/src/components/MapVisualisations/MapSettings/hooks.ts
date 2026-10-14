import { requireContext } from "@/Utils/requireContext";
import { createContext, useContext } from "react";
import type { UseFormReturn } from "react-hook-form";
import type { CameraOptions } from "../CameraControls/useCameraControls";
import type { LayerOptions } from "../LayerControls/useLayerControls";

export interface CameraSettings {
  form: UseFormReturn<CameraOptions>;
  options: CameraOptions;
}

// contexts live here (not in the provider file) so the .tsx only exports a component
// layer form and layer options are split: the form never changes, the options change on every edit
export const CameraSettingsContext = createContext<CameraSettings | undefined>(undefined);
export const LayerFormContext = createContext<UseFormReturn<LayerOptions> | undefined>(undefined);
export const LayerOptionsContext = createContext<LayerOptions | undefined>(undefined);

const providerName = "MapSettingsProvider";

export function useCameraSettings(): CameraSettings {
  return requireContext(useContext(CameraSettingsContext), "useCameraSettings", providerName);
}

export function useLayerForm(): UseFormReturn<LayerOptions> {
  return requireContext(useContext(LayerFormContext), "useLayerForm", providerName);
}

export function useLayerOptions(): LayerOptions {
  return requireContext(useContext(LayerOptionsContext), "useLayerOptions", providerName);
}
