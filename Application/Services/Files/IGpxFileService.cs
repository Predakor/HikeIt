using Domain.Common.Result;
using Domain.Trips.Entities.GpxFiles;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;
public interface IGpxFileService {
    Task<Result<GpxFile>> CreateAsync(IFormFile file, Guid userId, Guid tripId);
    Task<Result<AnalyticData>> ExtractGpxData(IFormFile file);
    Task<Result<AnalyticData>> ExtractGpxData(Guid id);
    Result<IFormFile> Validate(IFormFile file);

}

public abstract record GpxFileDto {

    public record Request(string Path) : GpxFileDto;
    public record New(string Path) : GpxFileDto;

}