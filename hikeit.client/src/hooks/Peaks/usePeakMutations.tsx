import api from "@/Utils/Api/apiRequest";
import type { AddPeakConfig } from "@/components/Peaks/AddPeak/addPeakFormConfig";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { config, detailedConfig } from "../Regions/UseRegionPeaks";
import { resolveCreated } from "@/Utils/Api/Resolvers/resolveCreated";
import type { RegionWithDetailedPeaks } from "@/types/ApiTypes/region.types";
import { useParams } from "react-router";
import { applyChanges } from "@/Utils/objectHelpers";

const basePath = "admin/peaks";

type UpdatePeak = {
  peakId: string | number;
  data: Partial<AddPeakConfig>;
};

type PostResult = {
  location: string;
};

export default function usePeakMutations() {
  const queryClient = useQueryClient();
  const { regionId } = useParams();

  const create = useMutation({
    mutationFn: (data: AddPeakConfig) =>
      api.post<PostResult>(`${basePath}/add`, data, resolveCreated),
    onSuccess: (_, { regionId }) => {
      const keys = config.queryKey(regionId!);

      queryClient.invalidateQueries({
        queryKey: keys,
      });

      queryClient.fetchQuery({
        queryKey: keys,
        queryFn: () => config.queryFn(regionId!),
      });
    },
  });

  const update = useMutation({
    mutationFn: ({ data, peakId }: UpdatePeak) =>
      api.patch<{}>(`${basePath}/${peakId}/update`, data),

    onSuccess: (_, { data: changes, peakId }) => {
      const key = detailedConfig.queryKey(regionId!);

      queryClient.setQueryData(key, (prev: RegionWithDetailedPeaks) => {
        if (!prev) {
          return prev;
        }

        const newPeaks = prev.peaks.map((p) =>
          p.id === peakId ? applyChanges(p, changes) : p
        );

        return {
          ...prev,
          peaks: newPeaks,
        };
      });

      queryClient.invalidateQueries({ queryKey: key });
    },
  });

  return {
    create,
    update,
  };
}
