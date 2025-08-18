using Domain.Common.Result;
using Domain.Common.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Commons.FileStorage;

public interface IFileStorage {
    Task<Result<Stream>> DownloadAsync(string path, BlobContainer type);
    Task<Result<FileReference>> UploadAsync(IFormFile file, string path, BlobContainer type);
    Task<Result<FileReference>> UpdateAsync(string path, BlobContainer type);
    Task<Result<bool>> DeleteAsync(string path, BlobContainer type);
}

public enum BlobContainer {
    Avatar,
    File,
}
