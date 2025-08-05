import { resolveCreated } from "@/Utils/Api/Resolvers/resolveCreated";
import api from "@/Utils/Api/apiRequest";
import { useMutation } from "@tanstack/react-query";
import useTripCache from "./useTripCache";
import { basePath } from "./useTrips";
import { useEffect, useRef } from "react";

interface TripUpdate {
  tripName: string;
  tripDay: string;
  regionId: number;
}

export function useTripDraft(file?: File) {
  if (!file) {
    return;
  }

  const { invalidateTripCachces, prefetchAndCache } = useTripCache();
  const drafInstance = useRef<string>("");

  useEffect(() => {
    const getDraftInstance = async () => {
      const request = await getDraft.mutateAsync();

      if (request) {
        const request2 = attachFile.mutateAsync(file);
        console.log("r2 " + request2);
      }

      console.log(request);
    };

    if (!getDraft.isPending) {
      getDraftInstance();
    }
  }, []);

  const getDraft = useMutation({
    mutationFn: () => api.post(`${basePath}/drafts/new`, null, resolveCreated),
    onSuccess: ({ location }) => {
      let path = location;
      if (location.charAt(0) === "/") {
        path = location.slice(1);
      }

      drafInstance.current = path;
      return path;
    },
  });

  const attachFile = useMutation({
    mutationFn: (file: File) => {
      const formData = new FormData();
      formData.append("file", file);
      console.log({ file, formData });

      return api.post(`${drafInstance.current}/file`, formData, resolveCreated);
    },
  });

  const update = useMutation({
    mutationFn: (update: Partial<TripUpdate>) =>
      api.patch(`${drafInstance.current}/`, update, resolveCreated),
  });

  const submit = useMutation({
    mutationFn: () =>
      api.post(`${drafInstance.current}/submit`, {}, resolveCreated),
    onSuccess: async ({ location }) => {
      invalidateTripCachces();
      if (location) {
        prefetchAndCache(location);
      }
    },
  });

  return { getDraft, update, attachFile, submit };
}
