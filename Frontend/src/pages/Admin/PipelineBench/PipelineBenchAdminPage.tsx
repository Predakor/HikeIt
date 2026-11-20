import AdminLayout from "@/Layout/AdminLayout";
import api from "@/Utils/Api/apiRequest";
import PipelineBench from "@/components/Trip/Details/PipelineBench/PipelineBench";
import { type SelectedTrip, TripPicker } from "@/components/Trip/Details/PipelineBench/TripPicker";
import { Text } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { useSearchParams } from "react-router";

const tripParamName = "trip";

export default function PipelineBenchAdminPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const tripId = searchParams.get(tripParamName);

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
        <PipelineBench tripId={tripId} />
      : <Text color={"fg.muted"}>Pick a trip above to preview and tune its pipeline config.</Text>}
    </AdminLayout>
  );
}
