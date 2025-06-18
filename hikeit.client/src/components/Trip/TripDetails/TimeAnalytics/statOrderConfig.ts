import StatGroup from "@/components/Stats/StatGroup";
import type { OrderConfig } from "@/types/Utils/OrderTypes";
import type { TimeAnalytic } from "../../Types/TripAnalyticsTypes";

export const timeAnalyticOrderConfig: OrderConfig<TimeAnalytic> = [
  {
    type: "group",
    label: "Activity",
    items: [
      { key: "activeTime", label: "" },
      { key: "idleTime", label: "" },
      { key: "ascentTime", label: "" },
      { key: "descentTime", label: "" },
    ],
  },
  {
    type: "group",
    label: "Speeds",
    items: [
      { key: "averageSpeedKph", label: "" },
      { key: "averageAscentKph", label: "" },
      { key: "averageDescentKph", label: "" },
    ],
  },

  {
    type: "group",
    label: "Time Frame",
    items: [
      { key: "duration", label: "" },
      { key: "startTime", label: "" },
      { key: "endTime", label: "" },
    ],
  },
];
