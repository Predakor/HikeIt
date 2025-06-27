import { apiPath } from "@/data/apiPaths";
import { resolveApiResponse, type ResponseResolver } from "./ApiClient";

async function apiRequest(path: string, options: RequestInit = {}) {
  const finalOptions: RequestInit = {
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
    credentials: "include",
    ...options,
  };
  return await fetch(apiPath + path, finalOptions);
}

const post = async <T>(
  path: string,
  body: any,
  responseResolver?: ResponseResolver<T>
): Promise<T> => {
  const request = await apiRequest(path, {
    method: "POST",
    body: JSON.stringify(body),
  });

  return responseResolver
    ? responseResolver(request)
    : resolveApiResponse(request);
};

const get = async <T>(
  path: string,
  params?: Record<string, any>,
  responseResolver?: ResponseResolver<T>
): Promise<T> => {
  const query = params ? `?${toQueryString(params)}` : "";
  var request = await apiRequest(path + query, { method: "GET" });

  return responseResolver
    ? responseResolver(request)
    : resolveApiResponse(request);
};

const api = {
  get,
  post,
} as const;

export default api;

function toQueryString(params: Record<string, any>): string {
  const query = new URLSearchParams();

  Object.entries(params).forEach(([key, value]) => {
    if (value !== undefined && value !== null) {
      query.append(key, String(value));
    }
  });

  return query.toString();
}
