import api from "@/Utils/Api/apiRequest";
import type { AddPeakConfig } from "@/components/Peaks/AddPeak/addPeakFormConfig";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { config } from "../Regions/UseRegionPeaks";
import { resolveCreated } from "@/Utils/Api/Resolvers/resolveCreated";

export default function usePeakMutations() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: AddPeakConfig) =>
      api.post<{ location: string }>("admin/peaks/add", data, resolveCreated),
    onSuccess: (data, { regionId }) => {
      const keys = config.queryKey(regionId!);

      queryClient.invalidateQueries({
        queryKey: keys,
      });

      queryClient.fetchQuery({
        queryFn: () => config.queryFn(regionId!),
        queryKey: keys,
      });

      console.log("added peak:" + data.location);
    },
  });
}
