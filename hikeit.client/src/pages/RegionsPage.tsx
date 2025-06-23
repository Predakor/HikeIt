import apiClient from "@/Utils/Api/ApiClient";
import Regions from "@/components/Regions/Regions";
import FetchWrapper from "@/components/Wrappers/Fetching";
import type { Region } from "@/data/types";
import { Box, Heading, SimpleGrid, Skeleton, Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

const staleTime = 3600 * 24 * 30;

function RegionsPage() {
  const request = useQuery<Region[]>({
    queryKey: ["regions"],
    queryFn: () => apiClient<Region[]>("regions"),
    staleTime: staleTime,
  });

  return (
    <Stack gap={8}>
      <Box placeItems={"center"}>
        <Heading size={"5xl"}>Regions</Heading>
      </Box>
      <FetchWrapper request={request} LoadingComponent={RegionSkeleton}>
        {(data) => (
          <SimpleGrid
            alignItems={"stretch"}
            justifyItems={"stretch"}
            minChildWidth={{ base: "full", lg: "sm" }}
            gap={8}
          >
            <Regions data={data} />
          </SimpleGrid>
        )}
      </FetchWrapper>
    </Stack>
  );
}

function RegionSkeleton() {
  return (
    <>
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
      <Skeleton height="200px" />
    </>
  );
}
export default RegionsPage;
