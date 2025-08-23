import api from "@/Utils/Api/apiRequest";
import UserStat from "@/components/User/Stats";
import { type UserStats } from "@/components/User/Stats/Utils/statTypes";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { SimpleGrid } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

export default function UserStatsPage() {
  const stats = useQuery<UserStats>({
    queryKey: ["user-stats"],
    queryFn: () => api.get<UserStats>("users/me/stats"),
    staleTime: 1000 * 60 * 60,
  });

  return (
    <FetchWrapper request={stats}>
      {(data) => {
        return (
          <SimpleGrid
            justifySelf={"center"}
            alignSelf={"center"}
            columns={{ lg: 2 }}
            gap={16}
            placeContent={"center"}
          >
            <UserStat.Totals stats={data.totals} />
            <UserStat.Location stats={data.locations} />
            <UserStat.Average locations={data.locations} totals={data.totals} />
            <UserStat.Metas metas={data.metas} />
          </SimpleGrid>
        );
      }}
    </FetchWrapper>
  );
}
