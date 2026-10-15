using Application.Commons.Services.Auth;
using Application.FileReferences;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.GpxFile.Services;
using Domain.FileReferences;

namespace Application.Trips.Analytics.Pipeline;

// Loads the stored GPX file of a trip the same way the dev preview endpoints do. Read-only.
public sealed class TripFilePipelineSource(
    IFileReferenceRepository fileReferenceRepository,
    IGpxFileService fileService,
    IGpxService gpxService,
    IAuthService authService
) : IPipelineSource
{
    public async Task<PipelineInput?> LoadAsync(Guid tripId, CancellationToken cancellationToken)
    {
        var userId = await authService.WithLoggedUserId();
        if (!userId.IsSuccess)
        {
            return null;
        }

        var file = await fileReferenceRepository
            .GetByIdAsync(tripId, cancellationToken)
            .BindAsync(fileService.GetAsync)
            .BindAsync(gpxService.ExtractGpxData);

        if (!file.IsSuccess)
        {
            return null;
        }

        return new PipelineInput($"{userId.Value}/{tripId}", file.Value!.Points);
    }
}
