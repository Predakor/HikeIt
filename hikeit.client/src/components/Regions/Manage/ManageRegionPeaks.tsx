import { IconPlus } from "@/Icons/Icons";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import AddPeakForm from "@/components/Peaks/AddPeak/AddPeakForm";
import type { AddPeakConfig } from "@/components/Peaks/AddPeak/addPeakFormConfig";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import Dialog from "@/components/ui/Dialog/Dialog";
import usePeakMutations from "@/hooks/Peaks/usePeakMutations";
import type { Peak, PeakWithLocation } from "@/types/ApiTypes/peak.types";
import type { RegionWithDetailedPeaks } from "@/types/ApiTypes/region.types";
import { Button, Table } from "@chakra-ui/react";
import { useState } from "react";

interface Props {
  region: RegionWithDetailedPeaks;
}

export default function ManageRegionPeaks({ region }: Props) {
  const [showForm, setShowForm] = useState(false);

  const addPeak = usePeakMutations();

  const upload = (data: AddPeakConfig) => {
    addPeak.mutate({
      ...data,
      regionId: region.region.id,
    });
  };

  return (
    <>
      <SimpleCard
        title="Peaks in the region"
        headerCta={<Button onClick={() => setShowForm(true)}>Add Peak</Button>}
      >
        <PeaksTable peaks={region.peaks} />
      </SimpleCard>
      <Dialog
        onOpenChange={({ open }) => setShowForm(open)}
        title="Add Peak to the region"
        open={showForm}
      >
        <AddPeakForm requestState={addPeak} onSubmit={upload} />
      </Dialog>
    </>
  );
}

function PeaksTable({ peaks }: { peaks: PeakWithLocation[] }) {
  return (
    <Table.Root variant={"outline"} size="lg" stickyHeader interactive>
      <Table.Header>
        <Table.Row>
          <Table.ColumnHeader>Name</Table.ColumnHeader>
          <Table.ColumnHeader>Height</Table.ColumnHeader>
          <Table.ColumnHeader>Latitude</Table.ColumnHeader>
          <Table.ColumnHeader>Longitude</Table.ColumnHeader>
          <Table.ColumnHeader />
        </Table.Row>
      </Table.Header>
      <Table.Body>
        {peaks.map((peak) => (
          <Table.Row key={peak.id}>
            <Table.Cell>{peak.name}</Table.Cell>
            <Table.Cell>{peak.height}</Table.Cell>
            <Table.Cell>{peak.lat}</Table.Cell>
            <Table.Cell>{peak.lon}</Table.Cell>
            <Table.Cell>{<IconPlus />}</Table.Cell>
          </Table.Row>
        ))}
      </Table.Body>
    </Table.Root>
  );
}
