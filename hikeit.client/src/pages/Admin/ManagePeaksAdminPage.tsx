import api from "@/Utils/Api/apiRequest";
import AdminPage from "@/components/Pages/AdminPage";
import PageTitle from "@/components/Titles/PageTitle";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import { Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

export default function ManagePeaksAdminPage() {
  const getPeaks = usePeaks();
  return (
    <AdminPage>
      <Stack>
        <PageTitle title="Manage Peaks"></PageTitle>
        <FetchWrapper request={getPeaks} />
      </Stack>
    </AdminPage>
  );
}

function usePeaks() {
  return useQuery({
    queryKey: ["peaks"],
    queryFn: () => api.get("peaks"),
    staleTime: 1000 * 60 * 60,
  });
}
