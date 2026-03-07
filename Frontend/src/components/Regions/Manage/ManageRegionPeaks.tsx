import AddPeakForm from "@/components/Peaks/AddPeak/AddPeakForm";
import type { AddPeakConfig } from "@/components/Peaks/AddPeak/addPeakFormConfig";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import Dialog from "@/components/ui/Dialog/Dialog";
import usePeakMutations from "@/hooks/Peaks/usePeakMutations";
import type { RegionWithDetailedPeaks } from "@/types/Api/region.types";
import { Button } from "@chakra-ui/react";
import { useState } from "react";
import { PeaksTable } from "./PeaksTable";

interface Props {
  region: RegionWithDetailedPeaks;
}

export default function ManageRegionPeaks({ region }: Props) {
  const [showForm, setShowForm] = useState(false);
  const { create } = usePeakMutations();

  const upload = (data: AddPeakConfig) => {
    create.mutate({
      ...data,
      regionId: region.region.id,
    });
  };

  const peaksWithRegion = region.peaks.map((p) => ({ ...p, regionId: region.region.id }));

  return (
    <>
      <SimpleCard
        title="Peaks in the region"
        headerCta={
          <Button colorPalette={"green"} onClick={() => setShowForm(true)}>
            Add Peak
          </Button>
        }
      >
        <PeaksTable peaks={peaksWithRegion} />
      </SimpleCard>

      <Dialog
        onOpenChange={({ open }) => setShowForm(open)}
        title="Add Peak to the region"
        open={showForm}
      >
        <AddPeakForm requestState={create} onSubmit={upload} />
      </Dialog>
    </>
  );
}
