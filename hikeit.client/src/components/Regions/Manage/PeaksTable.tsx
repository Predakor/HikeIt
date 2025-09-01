import { IconEdit } from "@/Icons/Icons";
import UpdatePeakForm from "@/components/Peaks/UpdatePeak/UpdatePeakForm";
import Dialog from "@/components/ui/Dialog/Dialog";
import { MutationResult } from "@/components/ui/Results/MutationResult";
import usePeakMutations from "@/hooks/Peaks/usePeakMutations";
import type { PeakWithLocation } from "@/types/Api/peak.types";
import { For, Table } from "@chakra-ui/react";
import type { UseMutationResult } from "@tanstack/react-query";
import { useState } from "react";

const headers = ["Name", "Height", "Latitude", "Longitude", ""];

export function PeaksTable({ peaks }: { peaks: PeakWithLocation[] }) {
  const { update } = usePeakMutations();
  const [peakToUpdate, setPeakToUpdate] = useState<PeakWithLocation>();

  return (
    <>
      <Table.Root variant={"outline"} size="lg" stickyHeader interactive>
        <Table.Header>
          <Table.Row>
            <For each={headers}>
              {(header) => <Table.ColumnHeader>{header}</Table.ColumnHeader>}
            </For>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <For each={peaks}>
            {(peak) => (
              <Table.Row
                key={peak.id}
                onClick={() => setPeakToUpdate(peak)}
                _hover={{ cursor: "pointer" }}
              >
                <Table.Cell>{peak.name}</Table.Cell>
                <Table.Cell>{peak.height}</Table.Cell>
                <Table.Cell>{peak.lat}</Table.Cell>
                <Table.Cell>{peak.lon}</Table.Cell>
                <Table.Cell>{<IconEdit />}</Table.Cell>
              </Table.Row>
            )}
          </For>
        </Table.Body>
      </Table.Root>

      <Dialog
        title="Edit peak"
        open={!!peakToUpdate}
        onOpenChange={() => setPeakToUpdate(undefined)}
      >
        <MutationResult
          requestState={update as UseMutationResult}
          succesMesage={{
            title: "Succes",
            description: "Peak was succesfuly updated",
          }}
        />
        <UpdatePeakForm
          peak={peakToUpdate}
          requestState={update as UseMutationResult}
          onSubmit={(d) =>
            update.mutate({ peakId: peakToUpdate?.id!, data: d })
          }
        />
      </Dialog>
    </>
  );
}
