import RenderFromConfig from "@/components/Utils/RenderWithOrder/RenderFromConfig";
import type { TripAnalytic } from "@/types/ApiTypes/TripDtos";
import { routeAnalyticsRenderConfig } from "./statRenderConfig";

export default function RouteAnalytics({ data }: { data: TripAnalytic }) {
  const orderConfig = routeAnalyticsRenderConfig;

  return <RenderFromConfig data={data} config={orderConfig} />;
}
