import api from "@/Utils/Api/apiRequest";
import AdminPage from "@/components/Pages/AdminPage";
import AddPeakForm, {
  type AddPeakConfig,
} from "@/components/Peaks/AddPeak/AddPeakForm";
import { PeakList } from "@/components/Peaks/Peak";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import Dialog from "@/components/ui/Dialog/Dialog";
import UseRegionPeaks from "@/hooks/Regions/UseRegionPeaks";
import type { RegionWithPeaks } from "@/types/ApiTypes/types";
import { Button, Stack } from "@chakra-ui/react";
import { useMutation } from "@tanstack/react-query";
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

  const addPeak = useMutation({
    mutationFn: (data: AddPeakConfig) => api.post("admin/peaks/add", data),
  });

  const upload = (data: AddPeakConfig): void => {
    data = { ...data, regionId: region.region.id };
    addPeak.mutate(data);
  };
  console.log(addPeak);

  return (
    <>
      <SimpleCard
        title="Peaks in the region"
        headerCta={<Button onClick={() => setShowForm(true)}>Add Trip</Button>}
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
        <AddPeakForm onSubmit={upload} />
      </Dialog>
    </>
  );
}
