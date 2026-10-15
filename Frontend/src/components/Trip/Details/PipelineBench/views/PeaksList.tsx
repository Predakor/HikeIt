import type { MatchedPeak } from "@/types/Api/pipelinePreview.types";
import { For, Stack, Table, Text } from "@chakra-ui/react";
import type { PeaksListProps } from "../bench.types";

type PeakStatus = "new" | "kept" | "unknown";

interface PreviewPeakRow {
  peak: MatchedPeak;
  status: PeakStatus;
}

interface PeakComparison {
  previewRows: PreviewPeakRow[];
  lost: MatchedPeak[];
}

const headers = ["Name", "Peak id", "Elevation (m)", "Distance to peak (m)", "Status"];

const statusColors: Record<PeakStatus, string> = {
  new: "fg.success",
  kept: "fg.muted",
  unknown: "fg.muted",
};

const statusLabels: Record<PeakStatus, string> = {
  new: "new",
  kept: "kept",
  unknown: "-",
};

export default function PeaksList({ baseline, preview }: PeaksListProps) {
  if (!baseline && !preview) {
    return <Text color={"fg.muted"}>No data</Text>;
  }

  const comparison = comparePeaks(baseline?.matched, preview?.matched);

  return (
    <Stack gap={4}>
      <Text fontSize={"sm"}>
        {`Candidates: baseline ${baseline?.candidateCount ?? "-"}, preview ${preview?.candidateCount ?? "-"}; Matched: baseline ${baseline?.matched.length ?? "-"}, preview ${preview?.matched.length ?? "-"}`}
      </Text>

      <Table.Root variant={"outline"} size="sm">
        <Table.Header>
          <Table.Row>
            <For each={headers}>
              {(header) => <Table.ColumnHeader key={header}>{header}</Table.ColumnHeader>}
            </For>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <For each={comparison.previewRows}>
            {(row) => <PreviewPeakTableRow key={row.peak.peakId} row={row} />}
          </For>
        </Table.Body>
      </Table.Root>

      {comparison.lost.length > 0 ? (
        <Stack gap={1}>
          <Text fontSize={"sm"} fontWeight={"semibold"}>
            {`Lost (matched in baseline only): ${comparison.lost.length}`}
          </Text>
          <For each={comparison.lost}>
            {(peak) => (
              <Text key={peak.peakId} fontSize={"sm"} color={"fg.error"}>
                {`${peak.name} (id ${peak.peakId}, ${Math.round(peak.elevation)} m)`}
              </Text>
            )}
          </For>
        </Stack>
      ) : null}
    </Stack>
  );
}

function PreviewPeakTableRow({ row }: { row: PreviewPeakRow }) {
  const { peak, status } = row;

  return (
    <Table.Row>
      <Table.Cell>{peak.name}</Table.Cell>
      <Table.Cell>{peak.peakId}</Table.Cell>
      <Table.Cell>{Math.round(peak.elevation)}</Table.Cell>
      <Table.Cell>{peak.distanceM.toFixed(1)}</Table.Cell>
      <Table.Cell color={statusColors[status]}>{statusLabels[status]}</Table.Cell>
    </Table.Row>
  );
}

// Matches peaks by peakId. A side that is missing cannot be compared, so statuses are unknown and nothing is lost.
function comparePeaks(
  baselinePeaks: MatchedPeak[] | undefined,
  previewPeaks: MatchedPeak[] | undefined,
): PeakComparison {
  const basePeaks = baselinePeaks ?? [];
  const currentPeaks = previewPeaks ?? [];
  const hasBaseline = baselinePeaks !== undefined;
  const isComparable = hasBaseline && previewPeaks !== undefined;

  const baselineIds = new Set(basePeaks.map((peak) => peak.peakId));
  const previewIds = new Set(currentPeaks.map((peak) => peak.peakId));

  const previewRows = currentPeaks.map((peak) => ({
    peak,
    status: statusOf(isComparable, baselineIds.has(peak.peakId)),
  }));

  const lost = isComparable ? basePeaks.filter((peak) => !previewIds.has(peak.peakId)) : [];

  return { previewRows, lost };
}

function statusOf(isComparable: boolean, isInBaseline: boolean): PeakStatus {
  if (!isComparable) {
    return "unknown";
  }

  return isInBaseline ? "kept" : "new";
}
