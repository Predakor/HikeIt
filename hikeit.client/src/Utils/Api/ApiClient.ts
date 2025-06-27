import { apiPath } from "@/data/apiPaths";

export type ResponseResolver<T> = (reponse: Response) => Promise<T>;

async function apiClient<T>(
  path: string,
  options: RequestInit = {},
  resolveWith?: ResponseResolver<T>
): Promise<T> {
  const finalOptions: RequestInit = {
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
    credentials: "include",
    ...options,
  };

  const response = await fetch(apiPath + path, finalOptions);

  if (resolveWith) {
    return resolveWith(response);
  }

  return resolveApiResponse<T>(response);
}

export async function resolveApiResponse<T>(response: Response) {
  if (response.status === 204) return null as T;

  if (response.status === 401) {
    console.warn("Unauthorized — redirecting to login");
    if (!window.location.pathname.startsWith("/auth")) {
      window.location.href = "/auth/login";
    }
  }

  if (!response.ok) {
    throw new Error(`API error: ${response.status} ${response.statusText}`);
  }

  const contentType = response.headers.get("content-type") || "";

  if (contentType.includes("application/json")) {
    return await response.json();
  }
}

export default apiClient;
