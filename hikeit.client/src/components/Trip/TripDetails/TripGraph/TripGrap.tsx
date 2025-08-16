import IsAdminUser from "@/Utils/IsAdminUser";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import useUser from "@/hooks/Auth/useUser";
import useResourceLink from "@/hooks/useResourceLink";
import type { ResourceUrl } from "@/types/ApiTypes/types";
import PreviewGraphDevOnly from "./Dev_PreviewGraph/PreviewGraphDevOnly";
import ElevationGraph from "./ElevationGraph";
import type { ChartData } from "./grap.types";

function TripGrap({ data }: { data: ResourceUrl }) {
  const getCurrentUser = useUser();
  const getElevationData = useResourceLink<ChartData>(data);

  return (
    <FetchWrapper request={getCurrentUser}>
      {(user) => (
        <FetchWrapper request={getElevationData}>
          {(d) => <Graph data={d} isAdmin={IsAdminUser(user)} />}
        </FetchWrapper>
      )}
    </FetchWrapper>
  );
}

function Graph({ data, isAdmin }: { data: ChartData; isAdmin: boolean }) {
  return !isAdmin ? (
    <ElevationGraph data={data} />
  ) : (
    <PreviewGraphDevOnly data={data} />
  );
}

export default TripGrap;
