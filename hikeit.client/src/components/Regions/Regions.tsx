import type { Region } from "@/data/types";
import useFetch from "@/hooks/useFetch";
import { SimpleGrid } from "@chakra-ui/react";
import RegionCard from "./Card/RegionCard";
import FetchWrapper from "../Wrappers/Fetching";
import MapWrapper from "../Wrappers/Mapping";

function Regions() {
  const request = useFetch<Region[]>("regions");

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
