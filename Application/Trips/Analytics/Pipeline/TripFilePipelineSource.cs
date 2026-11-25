using Application.Trips.GpxFile.Services;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Pipeline;

// The raw stored file of a trip, parsed.
public sealed record PipelineInput(IReadOnlyList<GpxPoint> RawPoints);

public interface IPipelineSource
{
    // Read-only. Fails when the trip or its file does not exist.
    Task<Result<PipelineInput>> LoadAsync(Guid tripId, CancellationToken cancellationToken);
}

public sealed class TripFilePipelineSource(ITripSourceService tripSource) : IPipelineSource
{
    public Task<Result<PipelineInput>> LoadAsync(Guid tripId, CancellationToken cancellationToken)
    {
        return tripSource
            .GetStoredAsync(tripId, cancellationToken)
            .MapAsync(data => new PipelineInput(data.Points));
    }
}
