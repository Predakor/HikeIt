import type { GpxArrayWithGains } from "@/types/Api/gpx.types";
import type { Func } from "@/types/Utils/func.types";

export const arrayUtils = {
  sum,
  average,
};

function sum<T>(items: T[], getValue: Func<T, number>, initValue = 0) {
  return items.reduce(
    (accumulator, current) => (accumulator += getValue(current)),
    initValue
  );
}

function average<T>(items: T[], getValue: Func<T, number>) {
  return sum(items, getValue) / items.length;
}

export function downsampleToMaxSize<T>(
  gpxArray: T[],
  maxSize: number = 500
): T[] {
  if (gpxArray.length <= maxSize) return gpxArray;
  const factor = Math.ceil(gpxArray.length / maxSize);
  return gpxArray.filter((_, index) => index % factor === 0);
}

export function calculateStats(gpxArray: GpxArrayWithGains) {
  let totalClimb = 0;
  let totalDescend = 0;
  let totalLength = 0;

  gpxArray.forEach((entry) => {
    if (!entry?.gains) {
      return;
    }

    const { plannarDist, eleDelta } = entry.gains;

    if (eleDelta > 0) {
      totalClimb += eleDelta;
    } else {
      totalDescend += eleDelta;
    }

    //3D distance
    totalLength += Math.sqrt(plannarDist ** 2 + eleDelta ** 2);
  });

  const [start, end] = getTimeRange();

  return {
    climbed: Math.floor(totalClimb),
    descended: Math.floor(totalDescend),
    distance: Math.floor(totalLength),
    duration: getDuration(start, end),
    startTime: start,
    endTime: end,
  };

  function getDuration(start: string | undefined, end: string | undefined) {
    if (start && end) {
      const startDate = new Date(start).getTime();
      const endDate = new Date(end).getTime();
      const durationMs = endDate - startDate;
      return durationMs / 1000 / 60;
    }
    return null;
  }

  function getTimeRange() {
    const startTime = gpxArray[0]?.time;
    const endTime = gpxArray[gpxArray.length - 1]?.time;

    return [startTime, endTime];
  }
}
