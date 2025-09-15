import IsAdminUser from "@/Utils/IsAdminUser";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import useUser from "@/hooks/Auth/useUser";
import useResourceLink from "@/hooks/Api/useResourceLink";
import type { ResourceUrl } from "@/types/Api/types";
import PreviewGraphDevOnly from "./Dev_PreviewGraph/PreviewGraphDevOnly";
import ElevationGraph from "./ElevationGraph";
import type { ChartData } from "./grap.types";
import { ResponsiveContainer } from "recharts";

function TripGrap({ data }: { data: ResourceUrl }) {
  const getCurrentUser = useUser();
  const getElevationData = useResourceLink<ChartData>(data);

  return (
    <FetchWrapper request={getCurrentUser}>
      {(user) => (
        <FetchWrapper request={getElevationData}>
          {(d) => (
            <ResponsiveContainer width={"100%"} aspect={4}>
              <Graph data={d} isAdmin={IsAdminUser(user)} />
            </ResponsiveContainer>
          )}
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
