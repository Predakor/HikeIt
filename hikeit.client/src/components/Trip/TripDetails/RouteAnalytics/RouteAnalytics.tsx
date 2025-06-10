import RenderFromConfig from "@/components/Utils/RenderWithOrder/RenderFromConfig";
import type { TripAnalytic } from "@/types/ApiTypes/TripDtos";
import { Stack } from "@chakra-ui/react";
import { routeOrderConfig } from "./routeOrderConfig";

export default function RouteAnalytics({ data }: { data: TripAnalytic }) {
  const orderConfig = routeOrderConfig;

  return (
    <Stack gap={4}>
      <RenderFromConfig data={data} config={routeOrderConfig} />
    </Stack>
  );
}
