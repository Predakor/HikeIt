import type { Gains, GpxArray } from "@/components/AddTripForm/AddTrip/types";
import { haversineDistance } from "./haversineDistance";

export function downsampleToMaxSize<T>(
  gpxArray: T[],
  maxSize: number = 500
): T[] {
  if (gpxArray.length <= maxSize) return gpxArray;
  const factor = Math.ceil(gpxArray.length / maxSize);
  return gpxArray.filter((_, index) => index % factor === 0);
}

export function smoothMedian(data: GpxArray, windowSize: number): GpxArray {
  const half = Math.floor(windowSize / 2);
  return data.map((point, i) => {
    const start = Math.max(0, i - half); //window start
    const end = Math.min(data.length, i + half + 1); //window end

    const window = data
      .slice(start, end)
      .map((p) => p.ele)
      .sort((a, b) => a - b);

    const median = window[Math.floor(window.length / 2)];
    return { ...point, ele: median };
  });
}

export function generateGains(gpxArray: GpxArray): GpxArray {
  return gpxArray.map((curr, i) => {
    if (i === 0) return { ...curr, gains: null };

    const prev = gpxArray[i - 1];

    const eleDelta = curr.ele - prev.ele;
    const horizontalDist = haversineDistance(
      prev.lat,
      prev.lon,
      curr.lat,
      curr.lon
    );

    const slope = (eleDelta / horizontalDist) * 100;

    const gains: Gains = {
      plannarDist: horizontalDist,
      eleDelta: eleDelta,
      slope: slope,
    };

    return {
      ...curr,
      gains,
    };
  }) as GpxArray;
}

export function calculateStats(gpxArray: GpxArray) {
  let totalClimb = 0;
  let totalDescend = 0;
  let totalLength = 0;

  gpxArray.forEach((entry) => {
    if (!entry?.gains) {
      return;
    }

    const { plannarDist: distFromPrev, eleDelta } = entry.gains;

    if (eleDelta > 0) {
      totalClimb += eleDelta;
    } else {
      totalDescend += eleDelta;
    }

    //3D distance
    totalLength += Math.sqrt(distFromPrev ** 2 + eleDelta ** 2);
  });

  return {
    climbed: Math.floor(totalClimb),
    descended: Math.floor(totalDescend),
    distance: Math.floor(totalLength),
    duration: getDuration(),
  };

  function getDuration() {
    const startTime = gpxArray[0]?.time;
    const endTime = gpxArray[gpxArray.length - 1]?.time;

    if (startTime && endTime) {
      const startDate = new Date(startTime).getTime();
      const endDate = new Date(endTime).getTime();
      const durationMs = endDate - startDate;
      return durationMs / 1000 / 60;
    }
    return null;
  }
}
