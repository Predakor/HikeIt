import { haversineDistance } from "@/Utils/haversineDistance";
import type { Gain, GpxArray } from "@/types/ApiTypes/TripDtos";

export async function mapFromFileToGpxArray(file: File) {
  const text = await file.text();
  const parser = new DOMParser();
  const xml = parser.parseFromString(text, "application/xml");

  return Array.from(xml.getElementsByTagName("trkpt")).map((pt) => ({
    lat: parseFloat(pt.getAttribute("lat") || "0"),
    lon: parseFloat(pt.getAttribute("lon") || "0"),
    ele: parseFloat(pt.getElementsByTagName("ele")[0]?.textContent || "0"),
    time: pt.getElementsByTagName("time")[0]?.textContent || "",
  })) as GpxArray;
}

export function calculatePointToPointGains(array: GpxArray) {
  return array.map((curr, i) => {
    if (i === 0) return { ...curr };

    const prev = array[i - 1];

    const eleDelta = curr.ele - prev.ele;
    const horizontalDist = haversineDistance(
      prev.lat,
      prev.lon,
      curr.lat,
      curr.lon
    );

    const slope = (eleDelta / horizontalDist) * 100;

    const gains: Gain = {
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

export function smoothWithMedian(array: GpxArray, medianSize: number) {
  const half = Math.floor(medianSize / 2);
  return array.map((point, i) => {
    const [start, end] = getWindowEdges(i, half, array);

    const window = array
      .slice(start, end)
      .map((p) => p.ele)
      .sort((a, b) => a - b);

    const median = window[Math.floor(window.length / 2)];
    return { ...point, ele: median };
  }) as GpxArray;
}

function getWindowEdges(i: number, half: number, array: GpxArray) {
  const start = Math.max(0, i - half);
  const end = Math.min(array.length, i + half + 1);
  return [start, end];
}
