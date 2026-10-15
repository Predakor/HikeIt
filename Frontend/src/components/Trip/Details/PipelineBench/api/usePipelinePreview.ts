import api from "@/Utils/Api/apiRequest";
import type { PipelinePreviewRequest, PipelinePreviewResponse } from "@/types/Api/pipelinePreview.types";
import { useMutation } from "@tanstack/react-query";
import { mockPipelinePreview } from "./pipelinePreviewMock";

// Set VITE_PIPELINE_MOCK=true in the env to serve the fixture instead of calling the backend.
const isPipelineMockEnabled = import.meta.env.VITE_PIPELINE_MOCK === "true";

export function usePipelinePreview(tripId?: string) {
  return useMutation({
    mutationFn: (request: PipelinePreviewRequest) =>
      isPipelineMockEnabled ?
        mockPipelinePreview(request)
      : api.post<PipelinePreviewResponse>(`trips/${tripId}/analytics/pipeline/preview`, request),
  });
}
