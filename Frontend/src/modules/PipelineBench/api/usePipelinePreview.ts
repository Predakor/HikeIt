import api from "@/Utils/Api/apiRequest";
import type { PipeStageResults } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import type {
  ComparisonTarget,
  PipelineConfig,
} from "@/modules/PipelineBench/shared/types/PipelineConfig";
import type { StageId } from "@/modules/PipelineBench/shared/types/StageId";
import { useMutation } from "@tanstack/react-query";

export interface PipelinePreviewRequest {
  config: PipelineConfig;
  stages?: StageId[] | null; // null = all stages
  target?: ComparisonTarget; // what the comparison run uses; default currentProd
  includeSteps?: boolean; // time per filter in each stage's telemetry
}

export interface PipelinePreviewResponse {
  results: PipeStageResults;
  comparison: PipeStageResults; // the same stages run for the requested target
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
