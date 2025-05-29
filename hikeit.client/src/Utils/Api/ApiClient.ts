import { apiPath } from "@/data/apiPaths";

async function apiClient<T>(
  path: string,
  options: RequestInit = {}
): Promise<T> {
  const token = localStorage.getItem("accessToken"); // or from context/store

  const finalOptions: RequestInit = {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(options.headers || {}),
    },
  };

  const response = await fetch(apiPath + path, finalOptions);

  if (!response.ok) {
    if (response.status === 401) {
      // Handle unauthorized — redirect to login, refresh token, etc.
      console.warn("Unauthorized! Redirecting to login...");
      // window.location.href = "/login";
    }
    throw new Error(`API error: ${response.status} ${response.statusText}`);
  }

  return response.json();
}
export default apiClient;
