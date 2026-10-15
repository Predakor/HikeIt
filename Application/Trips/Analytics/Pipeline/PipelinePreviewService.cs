using Application.Trips.Analytics.Pipeline.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Trips.Analytics.Pipeline;

// Runs the requested stages and the baseline (all-default config), caching each stage's output.
public sealed class PipelinePreviewService(
    IPipelineSource source,
    IEnumerable<IPipelineStage> stages,
    IMemoryCache cache
) : IPipelinePreviewService
{
    public Task<PipelinePreviewResponse?> PreviewAsync(
        Guid tripId,
        PipelinePreviewRequest request,
        CancellationToken cancellationToken
    )
    {
        // STUB (Phase 0 of BE-3b): implemented by BE-3b.
        throw new NotImplementedException();
    }
}
