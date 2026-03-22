interface MedianSmoothingConfig {
  type: "medianSmoothingFilter";
  value: {
    windowSize: number;
  };
}

interface EmaSmoothingConfig {
  type: "emaSmoothingFilter";
  value: {
    alpha: number;
  };
}
interface MaxSpikeFilter {
  type: "maxSpikeFilter";
  value: {
    maxSpike: number;
  };
}
interface RoundingPrecisionFilter {
  type: "roundingPrecisionFilter";
  value: {
    decimals: number;
  };
}
export type GpxFilter =
  | MedianSmoothingConfig
  | EmaSmoothingConfig
  | MaxSpikeFilter
  | RoundingPrecisionFilter;

export type FilterTypes = GpxFilter["type"];
