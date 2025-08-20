using Domain.Common.Result;
using Domain.FileReferences;
using Domain.FileReferences.ValueObjects;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files;
public interface IGpxFileService {
    Task<Result<FileReference>> CreateAsync(FileContent file, Guid userId, Guid tripId);
    Task<Result<string>> UploadAsync(Guid fileId, Guid userId);
    Task<Result<bool>> DeleteAsync(string path);
    Task<Result<AnalyticData>> ExtractGpxData(FileContent file);
    Task<Result<AnalyticData>> ExtractGpxData(Guid id);
    Task<Result<FileContent>> ValidateAndExtract(IFormFile file);
    Result<IFormFile> Validate(IFormFile file);


}

public abstract record GpxFileDto {
    public record Request(string Path) : GpxFileDto;
    public record New(string Path) : GpxFileDto;

}