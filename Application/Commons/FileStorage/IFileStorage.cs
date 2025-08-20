using Domain.Common.Result;
using Domain.Common.ValueObjects;

namespace Application.Commons.FileStorage;

public interface IFileStorage {
    Task<Result<Stream>> DownloadAsync(string path, BlobContainer type);
    Task<Result<FileReference>> UploadAsync(FileContent file, string path, BlobContainer type);
    Task<Result<FileReference>> UpdateAsync(string path, BlobContainer type);
    Task<Result<bool>> DeleteAsync(string path, BlobContainer type);
}

public enum BlobContainer {
    Avatar,
    File,
}
