import type { BlockStatus } from "../shared/model/bench.types";

// The "all" tab shows the worst status of any stage, so it still flags a stale or failed branch.
const statusPriority: BlockStatus[] = ["error", "running", "stale", "idle", "skipped", "done"];

export function worstStatus(statuses: BlockStatus[]): BlockStatus {
  return statusPriority.find((candidate) => statuses.includes(candidate)) ?? "idle";
}
