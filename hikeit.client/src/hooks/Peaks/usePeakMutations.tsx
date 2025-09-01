import { resolveCreated } from "@/Utils/Api/Resolvers/resolveCreated";
import api from "@/Utils/Api/apiRequest";
import { applyChanges } from "@/Utils/objectHelpers";
import type { AddPeakConfig } from "@/components/Peaks/AddPeak/addPeakFormConfig";
import type { RegionWithDetailedPeaks } from "@/types/Api/region.types";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "react-router";
import { detailedConfig } from "../Regions/UseRegionPeaks";

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

  const keys = detailedConfig.queryKey(regionId!);

  const create = useMutation({
    mutationFn: (data: AddPeakConfig) =>
      api.post<PostResult>(`${basePath}/add`, data, resolveCreated),
    onSuccess: (_, newPeak) => {
      queryClient.setQueryData(keys, (prev: RegionWithDetailedPeaks) => {
        return {
          ...prev,
          peaks: [...prev.peaks, newPeak],
        };
      });

      queryClient.invalidateQueries({
        queryKey: keys,
      });
    },
  });

  const update = useMutation({
    mutationFn: ({ data, peakId }: UpdatePeak) =>
      api.patch<{}>(`${basePath}/${peakId}/update`, data),

    onSuccess: (_, { data: changes, peakId }) => {
      queryClient.setQueryData(keys, (prev: RegionWithDetailedPeaks) => {
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

      queryClient.invalidateQueries({ queryKey: keys });
    },
  });

  return {
    create,
    update,
  };
}
