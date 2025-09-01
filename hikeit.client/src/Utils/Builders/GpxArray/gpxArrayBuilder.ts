import type { GpxArray, GpxArrayWithGains } from "@/types/Api/gpx.types";
import { calculateStats, downsampleToMaxSize } from "../../arrayUtils";
import {
  calculatePointToPointGains,
  mapFromFileToGpxArray,
  smoothWithMedian,
} from "./methods";

export class GpxArrayBuilder {
  private gpxArray: GpxArray = [];

  private constructor(initialData: GpxArray) {
    this.gpxArray = initialData;
  }

  static async fromFile(file: File): Promise<GpxArrayBuilder> {
    const parsed = await mapFromFileToGpxArray(file);
    return new GpxArrayBuilder(parsed);
  }

  smoothMedian(windowSize: number = 5) {
    this.gpxArray = smoothWithMedian(this.gpxArray, windowSize);
    return this;
  }

  generateGains() {
    this.gpxArray = calculatePointToPointGains(this.gpxArray);
    return this;
  }

  downsample(maxSize: number) {
    this.gpxArray = downsampleToMaxSize(this.gpxArray, maxSize);
    return this;
  }

  getStats() {
    return calculateStats(this.gpxArray as unknown as GpxArrayWithGains);
  }

  build() {
    return this.gpxArray;
  }
}
