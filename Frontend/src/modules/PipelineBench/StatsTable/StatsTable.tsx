import type { PipeStageResults } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import type { StageId } from "@/modules/PipelineBench/shared/types/StageId";
import { For, Show, Stack, Table } from "@chakra-ui/react";
import { Fragment, memo } from "react";
import { statsGroups } from "./statsRows";
import { noWrapCell, sourceIds, type SourceId } from "./statsTableLayout";
import { StatsTableRow } from "./StatsTableRow";

interface Props {
  stages?: StageId[];
  baseline: PipeStageResults;
  preview: PipeStageResults;
}

const sourceLabels: Record<SourceId, string> = {
  baseline: "Baseline",
  preview: "Preview",
};

const columns = [
  { label: "Metric", width: undefined },
  ...sourceIds.map((source) => ({ label: sourceLabels[source], width: "48" })),
];

// Memoised: it only depends on the results and the local reference choice, so editing a config must not re-render every row.
export const StatsTable = memo(function StatsTable({ stages, baseline, preview }: Props) {
  const sources: Record<SourceId, PipeStageResults> = { baseline, preview };
  const hasStages = stages === undefined;
  const groups =
    hasStages ? statsGroups : statsGroups.filter((group) => stages.includes(group.stage));
  const showGroupHeaders = hasStages || stages.length > 1;

  return (
    <Stack gap={2}>
      <Table.Root variant={"outline"} size="sm" tableLayout={"fixed"} w={"full"}>
        <Table.Header>
          <Table.Row>
            <For each={columns}>
              {(column) => (
                <Table.ColumnHeader key={column.label} w={column.width} {...noWrapCell}>
                  {column.label}
                </Table.ColumnHeader>
              )}
            </For>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <For each={groups}>
            {(group) => (
              <Fragment key={group.stage}>
                <Show when={showGroupHeaders}>
                  <Table.Row bg={"bg.subtle"}>
                    <Table.Cell
                      colSpan={columns.length}
                      fontWeight={"semibold"}
                      fontSize={"xs"}
                      textTransform={"uppercase"}
                    >
                      {group.label}
                    </Table.Cell>
                  </Table.Row>
                </Show>
                <For each={group.rows}>
                  {(row) => (
                    <StatsTableRow
                      key={`${group.stage}-${row.label}`}
                      row={row}
                      sources={sources}
                    />
                  )}
                </For>
              </Fragment>
            )}
          </For>
        </Table.Body>
      </Table.Root>
    </Stack>
  );
});
