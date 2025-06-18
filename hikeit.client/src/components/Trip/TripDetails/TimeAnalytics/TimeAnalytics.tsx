import RenderFromConfig from "@/components/Utils/RenderWithOrder";
import type { TimeAnalytic } from "../../Types/TripAnalyticsTypes";
import { timeAnalyticOrderConfig } from "./statOrderConfig";

function TimeAnalytics({ data }: { data: TimeAnalytic }) {
  return <RenderFromConfig config={timeAnalyticOrderConfig} data={data} />;
}
export default TimeAnalytics;
