import api from "@/Utils/Api/apiRequest";
import AdminPage from "@/components/Pages/AdminPage";
import { PeakList } from "@/components/Peaks/Peak";
import PageTitle from "@/components/Titles/PageTitle";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import type { Peak } from "@/types/ApiTypes/Analytics";
import { Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

export default function ManagePeaksAdminPage() {
  const getPeaks = usePeaks();
  return (
    <AdminPage>
      <Stack>
        <PageTitle title="Manage Peaks" />
        <FetchWrapper request={getPeaks}>
          {(peaks) => <PeakList peaks={peaks} />}
        </FetchWrapper>
      </Stack>
    </AdminPage>
  );
}

function usePeaks() {
  return useQuery({
    queryKey: ["peaks"],
    queryFn: () => api.get<Peak[]>("peaks"),
    staleTime: 1000 * 60 * 60,
  });
}
