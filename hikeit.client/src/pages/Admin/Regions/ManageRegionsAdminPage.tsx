import api from "@/Utils/Api/apiRequest";
import AdminPage from "@/components/Pages/AdminPage";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import LinkCard from "@/components/ui/Cards/LinkCard";
import type { Region } from "@/types/ApiTypes/types";
import { For } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

export default function ManageRegionsAdminPage() {
  const getRegions = useRegions();
  return (
    <AdminPage title="Manage Regions">
      <FetchWrapper request={getRegions}>
        {(regions) => (
          <For each={regions}>
            {(region) => (
              <LinkCard
                to={`${region.id}`}
                Header={region.id}
                Description={region.name}
              />
            )}
          </For>
        )}
      </FetchWrapper>
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
