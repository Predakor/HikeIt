import { apiPath } from "@/data/apiPaths";
import { useState } from "react";

export interface PostResponse {
  pending: boolean;
  result: string | null | Error;
}

type LazyFetch = (urlPath: PostPaths, body: BodyInit) => Promise<void>;

type PostPaths = "trips" | "users" | "files";

function usePost(): [LazyFetch, PostResponse] {
  const [pending, setPending] = useState(false);
  const [result, setResult] = useState<string | null>(null);

  const fetchData: LazyFetch = async (urlPath, body) => {
    setPending(true);
    setResult(null);

    const isFormData = body instanceof FormData;
    const headerType = isFormData
      ? undefined
      : {
          "Content-Type": "application/json",
        };

    const options: RequestInit = {
      method: "POST",
      headers: headerType,
      body: body,
    };

    const requestPath = apiPath + urlPath;

    try {
      const response = await fetch(requestPath, options);
      if (response.ok) {
        setResult("succes");
      }
    } catch (err) {
      if (err === typeof Error) setResult((err as Error)?.message);
    } finally {
      setPending(false);
    }
  };

  const postResponse: PostResponse = {
    pending,
    result,
  };

  return [fetchData, postResponse];
}

export default usePost;
