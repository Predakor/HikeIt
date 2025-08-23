import { lazy } from "react";

// not lazy import because barGraph is custom and doest not weight 600KB
export { BarGraph } from "./BarGraph";

export const LazyLineGraph = lazy(() => import("./LineChart"));
