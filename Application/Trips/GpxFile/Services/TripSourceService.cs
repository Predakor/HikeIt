using Application.FileReferences;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.FileReferences;
using Domain.FileReferences.ValueObjects;

namespace Application.Trips.GpxFile.Services;

public interface ITripSourceService
{
    Task<Result<AnalyticData>> GetStoredAsync(Guid tripId, CancellationToken cancellationToken = default);

    Task<Result<AnalyticData>> ExtractUploadedAsync(FileContent file);
}

public sealed class TripSourceService(
    IFileReferenceRepository fileReferenceRepository,
    IGpxFileService fileService,
    IGpxService gpxService
) : ITripSourceService
{
    public Task<Result<AnalyticData>> GetStoredAsync(Guid tripId, CancellationToken cancellationToken = default)
    {
        return fileReferenceRepository
            .GetByIdAsync(tripId, cancellationToken)
            .BindAsync(fileService.GetAsync)
            .BindAsync(gpxService.ExtractGpxData);
    }

    public Task<Result<AnalyticData>> ExtractUploadedAsync(FileContent file)
    {
        return gpxService.ExtractGpxData(file);
    }
}
