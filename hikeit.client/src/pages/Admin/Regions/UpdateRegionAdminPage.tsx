import AdminPage from "@/components/Pages/AdminPage";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { UseDetailedRegionPeaks } from "@/hooks/Regions/UseRegionPeaks";
import ManageRegionPeaks from "@components/Regions/Manage/ManageRegionPeaks";
import { useParams } from "react-router";

export default function ManageRegionAdminPage() {
  const { regionId } = useParams();
  const regionPeaks = UseDetailedRegionPeaks(Number(regionId));

  const regionName = regionPeaks?.data?.region.name || "";

  return (
    <AdminPage title={`Manage ${regionName}`}>
      <FetchWrapper request={regionPeaks}>
        {(region) => <ManageRegionPeaks region={region} />}
      </FetchWrapper>
    </AdminPage>
  );
}
