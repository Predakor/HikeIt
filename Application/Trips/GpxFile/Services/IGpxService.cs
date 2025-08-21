using Domain.Common.Result;
using Domain.FileReferences.ValueObjects;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Trips.GpxFile.Services;

public interface IGpxService {
    Task<Result<AnalyticData>> ExtractGpxData(FileContent file);
    Task<Result<AnalyticData>> ExtractGpxData(IFormFile file);
    Task<Result<AnalyticData>> ExtractGpxData(Stream stream);
}
