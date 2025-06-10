import RenderFromConfig from "@/components/Utils/RenderWithOrder";
import type { TimeAnalytics } from "../../Types/TripAnalyticsTypes";
import { timeAnalyticOrderConfig } from "./orderConfig";
import { Flex } from "@chakra-ui/react";

function TimeAnalytics({ data }: { data: TimeAnalytics }) {
  return (
    <Flex justifyItems={"center"} gap={8}>
      <RenderFromConfig config={timeAnalyticOrderConfig} data={data} />
    </Flex>
  );
}
export default TimeAnalytics;
