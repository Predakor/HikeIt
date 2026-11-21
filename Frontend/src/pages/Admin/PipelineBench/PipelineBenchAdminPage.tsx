import AdminLayout from "@/Layout/AdminLayout";
import api from "@/Utils/Api/apiRequest";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { usePipelineDefaults } from "@/modules/PipelineBench/api/usePipelineDefaults";
import PipelineBench from "@/modules/PipelineBench/PipelineBench";
import { type SelectedTrip, TripPicker } from "@/modules/PipelineBench/TripPicker";
import { Skeleton, Text } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { useSearchParams } from "react-router";

const tripParamName = "trip";

const DefaultsLoading = () => <Skeleton h={"60vh"} />;
const DefaultsError = () => (
  <Text color={"fg.error"}>Failed to load the production pipeline settings.</Text>
);

export default function PipelineBenchAdminPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const tripId = searchParams.get(tripParamName);
  const defaults = usePipelineDefaults(tripId ?? undefined);

  // Only the id lives in the URL, so the name and owner are looked up from it (also after a reload).
  const { data: trip } = useQuery({
    queryKey: ["admin-trip", tripId],
    queryFn: () => api.get<SelectedTrip>(`admin/trips/${tripId}`),
    enabled: tripId !== null,
  });

  const selectTrip = (next: SelectedTrip | null) => {
    const nextSearchParams = new URLSearchParams(searchParams);
    if (next) {
      nextSearchParams.set(tripParamName, next.id);
    } else {
      nextSearchParams.delete(tripParamName);
    }
    setSearchParams(nextSearchParams);
  };

  return (
    <AdminLayout
      title={"Pipeline config"}
      header={
        <TripPicker
          selected={tripId === null ? null : (trip ?? { id: tripId, name: "Loading…", owner: "" })}
          onSelect={selectTrip}
          onClear={() => selectTrip(null)}
        />
      }
    >
      {tripId ?
        <FetchWrapper
          request={defaults}
          LoadingComponent={DefaultsLoading}
          ErrorComponent={DefaultsError}
        >
          {(data) => <PipelineBench key={tripId} tripId={tripId} defaults={data} />}
        </FetchWrapper>
      : <Text color={"fg.muted"}>Pick a trip above to preview and tune its pipeline config.</Text>}
    </AdminLayout>
  );
}
