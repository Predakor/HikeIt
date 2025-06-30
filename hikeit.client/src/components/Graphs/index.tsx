import { lazy } from "react";

export const LazyBarGraph = lazy(() => import("./BarGraph"));
export const LazyLineGraph = lazy(() => import("./LineChart"));
