import { apiPath } from "@/data/apiPaths";
import type { Region } from "@/types/Api/region.types";
import { useRef } from "react";

function useRegionMatcher() {
  const regionsRef = useRef(new Map<number, Region>());

  const fetchRegionData = async (id: number): Promise<Region | undefined> => {
    try {
      const response = await fetch(`${apiPath}regions/${id}`, {
        method: "GET",
        headers: { "Content-Type": "application/json" },
      });

      if (response.ok) {
        const region = (await response.json()) as Region;
        regionsRef.current.set(region.id, region);
        return region;
      }
    } catch (error) {
      console.error("Fetch failed:", error);
    }
  };

  const matchRegion = async (id: number): Promise<Region | undefined> => {
    if (regionsRef.current.has(id)) {
      return regionsRef.current.get(id);
    }

    return await fetchRegionData(id);
  };

  return { matchRegion };
}

export default useRegionMatcher;
