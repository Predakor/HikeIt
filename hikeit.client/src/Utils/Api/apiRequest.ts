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
  const isFormData = body instanceof FormData;

  const request = await apiRequest(path, {
    method: "POST",
    body: isFormData ? body : JSON.stringify(body),
    headers: isFormData
      ? undefined // Let the browser set multipart/form-data with boundary
      : { "Content-Type": "application/json" },
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
  const request = await apiRequest(path + query, { method: "GET" });

  return responseResolver
    ? responseResolver(request)
    : resolveApiResponse(request);
};

const patch = async <T>(
  path: string,
  params?: Record<string, any>,
  responseResolver?: ResponseResolver<T>
): Promise<T> => {
  const body = JSON.stringify({ ...params });
  const request = await apiRequest(path, { method: "PATCH", body });

  return responseResolver
    ? responseResolver(request)
    : resolveApiResponse(request);
};

const put = async <T>(
  path: string,
  params?: Record<string, any>,
  responseResolver?: ResponseResolver<T>
): Promise<T> => {
  const query = params ? `?${toQueryString(params)}` : "";
  const request = await apiRequest(path + query, { method: "PUT" });

  return responseResolver
    ? responseResolver(request)
    : resolveApiResponse(request);
};

const api = {
  get,
  post,
  patch,
  put,
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
