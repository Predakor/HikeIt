using Domain.FileReferences.ValueObjects;

namespace Application.FileReferences;

public interface IFileStorage {
    Task<Result<Stream>> DownloadAsync(string path, BlobContainer type);
    Task<Result<SaveFileResponse>> UploadAsync(FileContent file, string path, BlobContainer type);
    Task<Result<bool>> DeleteAsync(string path, BlobContainer type);
}

public record FileCreationInfo(string Path, string Name);

public enum BlobContainer {
    Avatar,
    File,
}
