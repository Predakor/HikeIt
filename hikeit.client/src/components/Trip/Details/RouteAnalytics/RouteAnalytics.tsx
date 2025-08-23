import { SimpleGrid } from "@chakra-ui/react";
import { RouteAnalyticSections } from "./Sections";
import type { RouteAnalytic } from "@/types/ApiTypes/Analytics";

export type RouteAnalyticsProps = {
  data: RouteAnalytic;
};

export default function RouteAnalytics({ data }: RouteAnalyticsProps) {
  return (
    <SimpleGrid columns={{ base: 1, lg: 3 }} gap={8}>
      <RouteAnalyticSections.Distance data={data} />
      <RouteAnalyticSections.Elevation data={data} />
      <RouteAnalyticSections.Slope data={data} />
    </SimpleGrid>
  );
}
