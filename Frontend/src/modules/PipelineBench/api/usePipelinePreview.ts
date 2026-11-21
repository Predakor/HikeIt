import api from "@/Utils/Api/apiRequest";
import type {
  PipelineConfig,
  StageId,
  StageResults,
} from "@/modules/PipelineBench/api/pipelinePreview.types";
import { useMutation } from "@tanstack/react-query";

export interface PipelinePreviewRequest {
  config: PipelineConfig;
  stages?: StageId[] | null; // null = all stages
  includeRaw?: boolean; // ask for the raw run (untouched file); clients need it once per file
}

export interface PipelinePreviewResponse {
  results: StageResults;
  baseline: StageResults; // current production settings (cleanup values come from the GpxDataProcessing app setting)
  raw?: StageResults | null; // same stages on the untouched file; only present when includeRaw was requested
}

export function usePipelinePreview(tripId?: string) {
  return useMutation({
    mutationFn: ({ request, signal }: { request: PipelinePreviewRequest; signal: AbortSignal }) =>
      api.post<PipelinePreviewResponse>(
        `trips/${tripId}/analytics/pipeline/preview`,
        request,
        undefined,
        { signal },
      ),
  });
}
