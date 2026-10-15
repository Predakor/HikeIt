import type { PipelinePreviewRequest, PipelinePreviewResponse } from "@/types/Api/pipelinePreview.types";
// Raw import: the tsconfig does not enable resolveJsonModule, so the JSON is parsed by hand.
import fixtureRaw from "../../../../../../../docs/pipeline-preview-bench/fixture.response.json?raw";

const MOCK_DELAY_MS = 400;

const fixture = JSON.parse(fixtureRaw) as PipelinePreviewResponse;

// Ignores the request: always returns the sample response from docs/pipeline-preview-bench.
export function mockPipelinePreview(_request: PipelinePreviewRequest): Promise<PipelinePreviewResponse> {
  return new Promise((resolve) => {
    setTimeout(() => resolve(fixture), MOCK_DELAY_MS);
  });
}
