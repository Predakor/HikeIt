import type { OrderConfig } from "@/types/Utils/OrderTypes";
import type { TimeAnalytics } from "../../Types/TripAnalyticsTypes";

export const timeAnalyticOrderConfig: OrderConfig<TimeAnalytics> = [
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
