import AdminPage from "@/components/Pages/AdminPage";
import AddPeakForm from "@/components/Peaks/AddPeak/AddPeakForm";
import type { AddPeakConfig } from "@/components/Peaks/AddPeak/addPeakFormConfig";
import { PeakList } from "@/components/Peaks/Peak";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import Dialog from "@/components/ui/Dialog/Dialog";
import { toaster } from "@/components/ui/toaster";
import usePeakMutations from "@/hooks/Peaks/usePeakMutations";
import UseRegionPeaks from "@/hooks/Regions/UseRegionPeaks";
import type { RegionWithPeaks } from "@/types/ApiTypes/types";
import { Button, Stack } from "@chakra-ui/react";
import { useState } from "react";
import { useParams } from "react-router";

export default function ManageRegionAdminPage() {
  const params = useParams();
  const regionId = Number(params["regionId"]);
  const regionPeaks = UseRegionPeaks(regionId);

  const regionName = regionPeaks?.data?.region.name || "";

  return (
    <AdminPage title={`Manage ${regionName}`}>
      <FetchWrapper request={regionPeaks}>
        {(region) => <ManageRegionPeaks region={region} />}
      </FetchWrapper>
    </AdminPage>
  );
}

function ManageRegionPeaks({ region }: { region: RegionWithPeaks }) {
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
        <Stack gap={4}>
          <PeakList peaks={region.peaks} />
        </Stack>
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
