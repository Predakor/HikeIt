export type SourceId = "baseline" | "preview";

export const sourceIds: SourceId[] = ["baseline", "preview"];

export const reference: SourceId = "baseline";

// No line breaks keep every row the same height, so the table never shifts when values change.
export const noWrapCell = { whiteSpace: "nowrap", fontVariantNumeric: "tabular-nums" } as const;
