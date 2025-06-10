import type { OrderConfig } from "@/types/Utils/OrderTypes";
import { Flex } from "@chakra-ui/react";
import type { RouteAnalytic } from "../../Types/TripAnalyticsTypes";

export const routeOrderConfig: OrderConfig<RouteAnalytic> = [
  {
    type: "group",
    label: "distance",
    Wrapper: Flex,
    items: [
      {
        key: "totalDistanceKm",
        label: "Total distance",
      },
      { key: "totalAscent", label: "" },
      { key: "totalDescent", label: "" },
    ],
  },

  {
    type: "group",
    label: "elevation",
    Wrapper: Flex,
    items: [
      { key: "highestElevation", label: "" },
      { key: "lowestElevation", label: "" },
    ],
  },

  {
    type: "group",
    label: "slopes",
    Wrapper: Flex,
    items: [
      { key: "averageSlope", label: "" },
      { key: "averageAscentSlope", label: "" },
      { key: "averageDescentSlope", label: "" },
    ],
  },
];
