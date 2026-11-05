using Application.Commons.Services.Auth;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.GpxFile.Services;

namespace Application.Trips.Analytics.Pipeline;

public sealed class TripFilePipelineSource(ITripSourceService tripSource, IAuthService authService) : IPipelineSource
{
    public Task<PipelineInput?> LoadAsync(Guid tripId, CancellationToken cancellationToken)
    {
        return authService
            .WithLoggedUserId()
            .BindAsync(userId => tripSource
                .GetStoredAsync(tripId, cancellationToken)
                .MapAsync(data => new PipelineInput($"{userId}/{tripId}", data.Points)))
            .MatchAsync(input => (PipelineInput?)input, _ => default(PipelineInput));
    }
}
