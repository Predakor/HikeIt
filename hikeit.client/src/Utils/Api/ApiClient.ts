import { apiPath } from "@/data/apiPaths";

async function apiClient<T>(
  path: string,
  options: RequestInit = {}
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

  if (response.status === 204) return null as T;

  if (response.status === 401) {
    console.warn("Unauthorized — redirecting to login");
    if (window.location.pathname !== "/auth/login") {
      window.location.href = "/auth/login";
    }
    throw new Error("Unauthorized");
  }

  if (!response.ok) {
    throw new Error(`API error: ${response.status} ${response.statusText}`);
  }

  const contentType = response.headers.get("content-type") || "";

  if (contentType.includes("application/json")) {
    return response.json();
  }

  return null;
}

export default apiClient;
