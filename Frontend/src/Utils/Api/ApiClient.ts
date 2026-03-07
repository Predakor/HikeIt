import { apiPath } from "@/data/apiPaths";

export type ResponseResolver<T> = (reponse: Response) => Promise<T>;

console.log(apiPath);

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
    const body = (await response.json()) as { message: string; code: string };

    throw { title: body.code, message: body.message };
  }

  const contentType = response.headers.get("content-type") || "";

  if (contentType.includes("application/json")) {
    return await response.json();
  }

  if (contentType.includes("text/plain")) {
    return await response.text();
  }
}

export default apiClient;
