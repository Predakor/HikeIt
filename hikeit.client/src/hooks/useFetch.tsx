import { apiPath } from "@/data/apiPaths";
import { useEffect, useState } from "react";

export interface FetchResponse<T> {
  data: T | null;
  error: string | null;
  loading: boolean;
}

export type Route = "peaks" | "trips" | "users" | "regions";

function useFetch<T>(urlPath: Route, body?: any): FetchResponse<T> {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError(null);

      const options: RequestInit = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      };

      const requestPath = apiPath + urlPath;

      try {
        const response = await fetch(requestPath, options);
        if (!response.ok) {
          throw new Error(`Failed to fetch: ${response.statusText}`);
        }
        const result: T = await response.json();
        setData(result);
      } catch (err) {
        if (err === typeof Error) setError((err as Error).message);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [urlPath, body]);

  return { data, loading, error };
}

export default useFetch;
