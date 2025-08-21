using Domain.Common.Result;
using Domain.FileReferences;
using Domain.FileReferences.ValueObjects;

namespace Application.FileReferences;

public interface IGpxFileService {
    Task<Result<Stream>> GetAsync(FileReference refrence);
    Task<Result<string>> UploadAsync(Guid fileId, Guid userId);
    Task<Result<bool>> DeleteAsync(string path);

    Task<Result<FileReference>> CreateTemporrary(FileContent file, Guid userId, Guid tripId);
}

public abstract record GpxFileDto {
    public record Request(string Path) : GpxFileDto;

    public record New(string Path) : GpxFileDto;
}
