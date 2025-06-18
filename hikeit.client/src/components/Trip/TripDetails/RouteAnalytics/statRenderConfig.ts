import type { OrderConfig } from "@/types/Utils/OrderTypes";
import type { RouteAnalytic } from "../../Types/TripAnalyticsTypes";
import StatGroup from "@/components/Stats/StatGroup";

export const routeAnalyticsRenderConfig: OrderConfig<RouteAnalytic> = [
  {
    type: "group",
    label: "distance",
    RenderWith: StatGroup,
    items: [
      { key: "totalDistanceKm", label: "Total distance" },
      { key: "totalAscent", label: "" },
      { key: "totalDescent", label: "" },
    ],
  },

  {
    type: "group",
    label: "elevation",
    RenderWith: StatGroup,
    items: [
      { key: "highestElevation", label: "" },
      { key: "lowestElevation", label: "" },
    ],
  },

  {
    type: "group",
    label: "slopes",
    RenderWith: StatGroup,
    items: [
      { key: "averageSlope", label: "" },
      { key: "averageAscentSlope", label: "" },
      { key: "averageDescentSlope", label: "" },
    ],
  },
];
