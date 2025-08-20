using Domain.Common.Result;
using Domain.FileReferences.ValueObjects;

namespace Application.Commons.FileStorage;

public interface IFileStorage {
    Task<Result<Stream>> DownloadAsync(string path, BlobContainer type);
    Task<Result<SaveFileResponse>> UploadAsync(FileContent file, string path, BlobContainer type);
    Task<Result<bool>> DeleteAsync(string path, BlobContainer type);
}

public enum BlobContainer {
    Avatar,
    File,
}
