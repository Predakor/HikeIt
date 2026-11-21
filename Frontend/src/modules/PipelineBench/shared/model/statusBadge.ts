import type { BlockStatus } from "./bench.types";

export const statusBadge: Record<BlockStatus, { label: string; colorPalette: string }> = {
  idle: { label: "Idle", colorPalette: "gray" },
  stale: { label: "Stale", colorPalette: "orange" },
  running: { label: "Running", colorPalette: "blue" },
  done: { label: "Done", colorPalette: "green" },
  error: { label: "Error", colorPalette: "red" },
  skipped: { label: "Skipped", colorPalette: "gray" },
};
