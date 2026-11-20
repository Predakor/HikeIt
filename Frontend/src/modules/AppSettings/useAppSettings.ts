import api from "@/Utils/Api/apiRequest";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { AppSettingsEntry } from "./appSettingsSchema";

interface UpdateRequest {
  id: number;
  value: Record<string, unknown>;
}

export const config = {
  basePath: "appSettings",
  key: ["settings"],
  fetch: () => api.get<AppSettingsEntry[]>("appSettings"),
};

export function useAll() {
  return useQuery({
    queryKey: config.key,
    queryFn: config.fetch,
  });
}

export function useUpdate() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, value }: UpdateRequest) =>
      api.put(`${config.basePath}/${id}`, { body: JSON.stringify(value) }),
    onSuccess: (_, { id, value }) => {
      queryClient.setQueryData<AppSettingsEntry[]>(config.key, (settings) =>
        settings?.map((setting) => (setting.id === id ? { ...setting, value } : setting)),
      );
      queryClient.invalidateQueries({ queryKey: ["pipeline-defaults"] });
    },
  });
}
