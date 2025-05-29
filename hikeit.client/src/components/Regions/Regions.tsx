import apiClient from "@/Utils/Api/ApiClient";
import type { Region } from "@/data/types";
import { SimpleGrid } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import FetchWrapper from "../Wrappers/Fetching";
import MapWrapper from "../Wrappers/Mapping";
import RegionCard from "./Card/RegionCard";

const staleTime = 3600 * 24 * 30;
function Regions() {
  const request = useQuery<Region[]>({
    queryKey: ["regions"],
    queryFn: () => apiClient("regions"),
    staleTime: staleTime,
  });

  return (
    <FetchWrapper request={request}>
      {(data) => (
        <SimpleGrid
          width={{ base: "100vw", lg: "80vw" }}
          minChildWidth={{ base: "full", lg: "1/4" }}
          gap={"1em"}
        >
          <MapWrapper
            items={data}
            renderItem={(itemData) => (
              <RegionCard region={itemData} key={itemData.id} />
            )}
          />
        </SimpleGrid>
      )}
    </FetchWrapper>
  );
}
export default Regions;
