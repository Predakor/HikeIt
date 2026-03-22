import IsAdminUser from "@/Utils/IsAdminUser";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import useUser from "@/hooks/Auth/useUser";
import RouteVisualisation from "./RouteVisualisation";
import DevRouteVisualisation from "./Dev/DEV_RouteVisualisationt";
import type { ResourceUrl } from "@/types/Api/types";

export default function TripMap({ data }: { data: ResourceUrl }) {
  const getCurrentUser = useUser();

  return (
    <FetchWrapper request={getCurrentUser}>
      {(currentUser) =>
        IsAdminUser(currentUser) ?
          <DevRouteVisualisation data={data} />
        : <RouteVisualisation data={data} />
      }
    </FetchWrapper>
  );
}
