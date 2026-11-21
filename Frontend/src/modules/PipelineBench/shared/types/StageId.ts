// Mirrors Application/Trips/Analytics/Pipeline/Contracts/PipelineStages.cs.
export type StageId =
  | "cleanup"
  | "terrain"
  | "pace"
  | "elevationProfile"
  | "peakMatching"
  | "route";

export const allStageIds: StageId[] = [
  "cleanup",
  "terrain",
  "pace",
  "elevationProfile",
  "peakMatching",
  "route",
];

// Every stage depends on cleanup; pace also needs the terrain totals. Mirrors IPipelineStage.DependsOn.
export const stageDependencies: Record<StageId, StageId[]> = {
  cleanup: [],
  terrain: ["cleanup"],
  pace: ["cleanup", "terrain"],
  elevationProfile: ["cleanup"],
  peakMatching: ["cleanup"],
  route: ["cleanup"],
};
