import MapWrapper from "@/components/Wrappers/Mapping/MapWrapper";
import type { OrderConfig } from "@/types/Utils/OrderTypes";
import { SimpleGrid } from "@chakra-ui/react";
import RenderEntry from "./GenericRenderer/GenericRenderer";

export interface RenderOrderConfig<TFor extends object> {
  data: TFor;
  config: OrderConfig<TFor>;
}

export default function RenderFromConfig<T extends object>(
  props: RenderOrderConfig<T>
) {
  const { data, config } = props;

  return (
    <MapWrapper
      items={config}
      Wrapper={({ children }) => (
        <SimpleGrid
          gridTemplateColumns={{ base: "1", lg: "repeat(2,1fr)" }}
          w={"full"}
          gap={4}
        >
          {children}
        </SimpleGrid>
      )}
      renderItem={(entry, i) => (
        <RenderEntry entry={entry} data={data} key={i} />
      )}
    />
  );
}
