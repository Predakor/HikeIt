import api from "@/Utils/Api/apiRequest";
import AdminPage from "@/components/Pages/AdminPage";
import RegionCard from "@/components/Regions/Card/RegionCard";
import PageTitle from "@/components/Titles/PageTitle";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import type { Region } from "@/types/ApiTypes/types";
import { For, Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

export default function ManageRegionsAdminPage() {
  const getRegions = useRegions();
  return (
    <AdminPage>
      <Stack>
        <PageTitle title="Manage Peaks" />
        <FetchWrapper request={getRegions}>
          {(regions) => (
            <For each={regions}>
              {(region) => (
                <RegionCard Header={region.id} Description={region.name} />
              )}
            </For>
          )}
        </FetchWrapper>
      </Stack>
    </AdminPage>
  );
}

function useRegions() {
  return useQuery({
    queryKey: ["regions"],
    queryFn: () => api.get<Region[]>("regions"),
    staleTime: 1000 * 60 * 60,
  });
}
