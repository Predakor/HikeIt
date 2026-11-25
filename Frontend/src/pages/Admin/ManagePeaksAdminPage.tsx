import api from "@/Utils/Api/apiRequest";
import AdminLayout from "@/Layout/AdminLayout";
import { PeakList } from "@/components/Peaks/Peak";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import type { Peak } from "@/types/Api/peak.types";
import { useQuery } from "@tanstack/react-query";

export default function ManagePeaksAdminPage() {
  const getPeaks = usePeaks();
  return (
    <AdminLayout title="Manage Peaks">
      <FetchWrapper request={getPeaks}>{(peaks) => <PeakList peaks={peaks} />}</FetchWrapper>
    </AdminLayout>
  );
}

function usePeaks() {
  return useQuery({
    queryKey: ["peaks"],
    queryFn: () => api.get<Peak[]>("peaks"),
    staleTime: 1000 * 60 * 60,
  });
}
