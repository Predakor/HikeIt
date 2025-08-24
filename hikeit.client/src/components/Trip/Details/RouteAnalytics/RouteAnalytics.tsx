import type { RouteAnalytic } from "@/types/ApiTypes/analytics.types";
import { Grid, GridItem } from "@chakra-ui/react";
import { RouteAnalyticSections } from "./Sections";

export type RouteAnalyticsProps = {
  data: RouteAnalytic;
};

export default function RouteAnalytics({ data }: RouteAnalyticsProps) {
  return (
    <Grid
      display={{ base: "flex", lg: "grid" }}
      flexDirection={"column"}
      templateRows={"repeat(6, 1fr)"}
      templateColumns={{ lg: "repeat(5, 1fr)" }}
      gap={8}
    >
      <GridItem colSpan={2} rowSpan={2}>
        <RouteAnalyticSections.Distance data={data} />
      </GridItem>

      <GridItem colSpan={3} rowSpan={6}>
        <RouteAnalyticSections.Elevation data={data} />
      </GridItem>

      <GridItem colSpan={2} rowSpan={4}>
        <RouteAnalyticSections.Slope data={data} />
      </GridItem>
    </Grid>
  );
}
